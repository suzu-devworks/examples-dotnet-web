#!/bin/sh
set -eu

SCRIPT_DIR=$(CDPATH= cd -- "$(dirname "$0")" && pwd)
ROOT_DIR=$(CDPATH= cd -- "$SCRIPT_DIR/.." && pwd)

if [ -f "$ROOT_DIR/scripts/update-serena-memory.sh" ]; then
  /bin/sh "$ROOT_DIR/scripts/update-serena-memory.sh"
fi