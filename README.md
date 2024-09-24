# Batch Rename Project 
## Overview

The Batch Rename project is a tool designed to automate the renaming of files in bulk. It becomes the most necessary when a huge amount of files must obey specific naming patterns. This project provides a modern GUI for users to rename the required files, preview changes, and then rename them in one go, hence it is a useful tool in changing the names of multiple files.

## How to use
**1**. Select all files and folders you want to rename.

**2**. Create a set of rules for renaming the files. 

    - Each rule can be added from a menu.
    - Each rule's parameters can be edited.
    
**3**. Apply the set of rules in numerical order to each file, make them have a new name.

**4**. Save this set of rules into presets for quickly loading later if you need to reuse.

## Renaming Rules
1. Change the extension to another extension (no conversion, force renaming extension).
2. Add counter to the end of the file.
    - [ ]  Could specify the start value, steps, number of digits (Could have padding like 01, 02, 03...10....99).
3. Remove all space from the beginning and the ending of the filename.
4. Replace certain characters into one character like replacing "-" ad "_" into space " ".
    - [ ]  Could be the other way like replace all space " " into dot "."
5. Adding a prefix to all the files.
6. Adding a suffix to all the files.
7. Convert all characters to lowercase, remove all spaces.
8. Convert filename to PascalCase.
