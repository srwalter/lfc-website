#!/bin/bash
# Deploy LFC website to FTP server (206.196.6.138)
# Requires: mono-msbuild installed via sudo pacman -S mono-msbuild mono-msbuild-sdkresolver

set -e

ROOT="$(dirname "$0")"
PROJECT="$ROOT/LFC"
PUBLISH="$ROOT/publish"

# 1. Restore packages (already done, but safe to re-run)
nuget restore "$ROOT/LFC.sln"

# 2. Build Production configuration
msbuild "$ROOT/LFC.sln" /p:Configuration=Production /p:Platform="Any CPU" /v:minimal

# 3. Copy publish output (FileSystem publish since mono-msbuild doesn't support FTP directly)
rm -rf "$PUBLISH"
mkdir -p "$PUBLISH"

cp $ROOT/Web.config $PROJECT
cp -r "$PROJECT/Web.config" "$PROJECT/Web.Production.config" "$PROJECT/Global.asax" "$PUBLISH/"
cp -r "$PROJECT/App_Start" "$PROJECT/DAL" "$PROJECT/Controllers" "$PROJECT/Models" "$PROJECT/ViewModels" "$PUBLISH/"
cp -r "$PROJECT/Migrations" "$PUBLISH/"
cp -r "$PROJECT/Views" "$PUBLISH/"
cp -r "$PROJECT/Content" "$PROJECT/Scripts" "$PUBLISH/"
cp -r "$PROJECT/bin" "$PUBLISH/"

# 4. Deploy via FTP — prompt for password
read -rsp "FTP password: " FTP_PASSWORD
echo
[ -z "$FTP_PASSWORD" ] && { echo "Aborted"; exit 1; }

lftp -u Lexingtonflyingclub,$FTP_PASSWORD 206.196.6.138 <<EOF
set ftp:passive-mode yes
mirror --reverse --parallel=8 $PUBLISH /
bye
EOF

echo "Deploy complete to http://data.lexingtonflyingclub.org"
