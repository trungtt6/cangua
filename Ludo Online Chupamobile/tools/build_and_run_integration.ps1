param(
    [string]$unityPath = "C:\\Program Files\\Unity\\Hub\\Editor\\2021.3.0f1\\Editor\\Unity.exe",
    [string]$projectPath = "C:\\deps\\cangua\\Ludo Online Chupamobile",
    [int]$clients = 4,
    [string]$buildOutputDir = "$env:USERPROFILE\\Desktop\\LudoIntegrationBuild",
    [ValidateSet("Win","Linux","Mac")][string]$buildTarget = "Win",
    [int]$testTimeout = 180,
    [string]$uploadUrl = "" # optional: HTTP endpoint to POST zipped logs
)

# Build the project once using Unity CLI
Write-Host "Building project..."

# Build with output dir and target
$buildArgs = "-buildOutput `"$buildOutputDir`" -buildTarget $buildTarget"
& "${unityPath}" -quit -batchmode -projectPath "${projectPath}" -executeMethod IntegrationBuild.PerformIntegrationBuild $buildArgs -logFile build_log.txt

# Determine built executable based on platform
if ($buildTarget -eq 'Linux') {
    $buildExe = Join-Path $buildOutputDir "LudoIntegration.x86_64"
} elseif ($buildTarget -eq 'Mac') {
    $buildExe = Join-Path $buildOutputDir "LudoIntegration.app"
} else {
    $buildExe = Join-Path $buildOutputDir "LudoIntegration.exe"
}

if (!(Test-Path $buildExe)) {
    Write-Error "Build output not found: $buildExe. Check build_log.txt for details.";
    exit 1
}

Write-Host "Build succeeded: $buildExe"

# Launch multiple clients
$processes = @()
$logsDir = Join-Path $buildOutputDir "integration_logs"
if (!(Test-Path $logsDir)) { New-Item -ItemType Directory -Path $logsDir | Out-Null }
# Prepare per-client build copies to avoid file locking
$clientRoots = @{}
for ($i = 1; $i -le $clients; $i++) {
    $clientFolder = Join-Path $buildOutputDir ("client_$i")
    if (Test-Path $clientFolder) { Remove-Item -Recurse -Force $clientFolder }
    New-Item -ItemType Directory -Path $clientFolder | Out-Null

    if ($buildTarget -eq 'Mac') {
        # copy the .app bundle
        $dest = Join-Path $clientFolder (Split-Path $buildExe -Leaf)
        Copy-Item -Recurse -Force $buildExe -Destination $dest
        $clientExe = $dest
    } else {
        # copy exe/binary and _Data folder if present
        $exeName = Split-Path $buildExe -Leaf
        $destExe = Join-Path $clientFolder $exeName
        Copy-Item -Force $buildExe -Destination $destExe
        # copy Data folder
        $dataFolder = $buildExe + "_Data"
        if (Test-Path $dataFolder) {
            Copy-Item -Recurse -Force $dataFolder -Destination (Join-Path $clientFolder (Split-Path $dataFolder -Leaf))
        }
        $clientExe = $destExe
    }

    $clientRoots[$i] = @{ Exe = $clientExe; Folder = $clientFolder }
}

# Launch multiple clients from their per-client folders
$processes = @()
$procStartTimes = @{}
for ($i = 1; $i -le $clients; $i++) {
    $clientLog = Join-Path $logsDir "client_$i.log"
    $playerName = "CI_Client_$i"
    $args = "-auto_integration -clientId $i -playerName $playerName -logFile $clientLog"
    $clientExe = $clientRoots[$i].Exe
    $clientFolder = $clientRoots[$i].Folder

    Write-Host "Starting client $i from $clientFolder"
    if ($buildTarget -eq 'Mac') {
        $p = Start-Process -FilePath "open" -ArgumentList "-W", $clientExe, "--args", $args -WorkingDirectory $clientFolder -PassThru
    } else {
        $p = Start-Process -FilePath $clientExe -ArgumentList $args -WorkingDirectory $clientFolder -PassThru
    }
    $processes += $p
    $procStartTimes[$p.Id] = Get-Date
}

# Wait for processes to exit or timeout
$watch = [System.Diagnostics.Stopwatch]::StartNew()
while ($watch.Elapsed.TotalSeconds -lt $testTimeout) {
    $allExited = $true
    foreach ($p in $processes) {
        $p.Refresh()
        if (-not $p.HasExited) { $allExited = $false; break }
    }
    if ($allExited) { break }
    Start-Sleep -Seconds 2
}

# Kill any remaining
foreach ($p in $processes) {
    if (-not $p.HasExited) {
        Write-Host "Killing process $($p.Id)"
        try { $p.Kill() } catch {}
    }
}

Write-Host "Integration run complete. Logs in: $logsDir"

# Collect logs and run lightweight parser
$results = @()
Get-ChildItem -Path $logsDir -Filter *.log | ForEach-Object {
    $logName = $_.Name
    Write-Host "---- $logName ----"
    $content = Get-Content $_.FullName
    # Print tail
    $content | Select-Object -Last 200 | ForEach-Object { Write-Host $_ }

    # Parse for success token
    $succeeded = $false
    foreach ($line in $content) {
        if ($line -like '*[TEST_TOKEN]*gameSceneStarted*') { $succeeded = $true; break }
        if ($line -like '*AutoIntegrationRunner: game scene started*') { $succeeded = $true; break }
    }

    # Determine client id from filename
    if ($logName -match 'client_(\d+)') { $cid = [int]$Matches[1] } else { $cid = 0 }

    # find process by startTime map: approximate by client index
    $proc = $processes | Where-Object { $_.Id -and $_.Id -eq ($processes[$cid-1].Id) }
    $startTime = $null
    if ($proc -ne $null -and $proc.Id -ne $null -and $procStartTimes.ContainsKey($proc.Id)) {
        $startTime = $procStartTimes[$proc.Id]
    } else {
        $startTime = Get-Date
    }
    $endTime = Get-Date
    $duration = ($endTime - $startTime).TotalSeconds

    $results += [PSCustomObject]@{
        Client = $cid
        Log = $_.FullName
        Succeeded = $succeeded
        Duration = [math]::Round($duration,2)
    }
}

# Generate JUnit XML report
$tests = $results.Count
$failures = ($results | Where-Object { -not $_.Succeeded }).Count
$totalTime = [math]::Round(($results | Measure-Object -Property Duration -Sum).Sum,2)
$xml = New-Object System.Xml.XmlDocument
$declaration = $xml.CreateXmlDeclaration("1.0","utf-8",$null)
$xml.AppendChild($declaration) | Out-Null

$testsuites = $xml.CreateElement("testsuites")
$xml.AppendChild($testsuites) | Out-Null

$testsuite = $xml.CreateElement("testsuite")
$testsuite.SetAttribute("name","Integration")
$testsuite.SetAttribute("tests",$tests)
$testsuite.SetAttribute("failures",$failures)
$testsuite.SetAttribute("time",$totalTime)
$testsuites.AppendChild($testsuite) | Out-Null

foreach ($r in $results) {
    $tc = $xml.CreateElement("testcase")
    $tc.SetAttribute("classname","integration")
    $tc.SetAttribute("name","client_$($r.Client)")
    $tc.SetAttribute("time",$r.Duration)
    if (-not $r.Succeeded) {
        $failure = $xml.CreateElement("failure")
        $failure.SetAttribute("message","Client did not reach game scene within timeout")
        $failure.AppendChild($xml.CreateTextNode("See log: $($r.Log)")) | Out-Null
        $tc.AppendChild($failure) | Out-Null
    }
    $testsuite.AppendChild($tc) | Out-Null
}

$reportPath = Join-Path $buildOutputDir "junit_integration_report.xml"
$xml.Save($reportPath)
Write-Host "JUnit report written to: $reportPath"

# Zip logs
$zipPath = Join-Path $buildOutputDir "integration_logs.zip"
if (Test-Path $zipPath) { Remove-Item $zipPath }
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::CreateFromDirectory($logsDir, $zipPath)
Write-Host "Logs zipped to: $zipPath"

# Optional upload
if ($uploadUrl -ne "") {
    Write-Host "Uploading logs to $uploadUrl"
    try {
        $wc = New-Object System.Net.WebClient
        $wc.UploadFile($uploadUrl, $zipPath)
        Write-Host "Upload complete"
    } catch {
        Write-Warning "Upload failed: $_"
    }
}

# Exit code: 0 success, 2 test failures
if ($failures -eq 0) { exit 0 } else { exit 2 }
