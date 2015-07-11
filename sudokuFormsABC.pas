{$reference 'core.dll'}

uses 
    System,
    FormsABC,
    LousySudoku;

const
    windowTitle : string = 'LousySudoku v1';
var
    theSudoku : Sudoku;
begin
    MainForm.Title := windowTitle;
end.