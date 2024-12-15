# setup_pre-commit.ps
# This script initializes pre-commit hooks for the repository

# Ensure pre-commit is installed
if (-not (Get-Command pre-commit -ErrorAction SilentlyContinue)) {
    Write-Host "pre-commit is not installed. Installing..."
    pip install pre-commit
}

# Navigate to the repository root
$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $repoRoot

# Install pre-commit hooks
Write-Host "Installing pre-commit hooks..."
pre-commit install --hook-type pre-commit --hook-type pre-push --hook-type commit-msg

Write-Host "Pre-commit hooks installed successfully."
