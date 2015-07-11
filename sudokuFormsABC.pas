{$reference 'core.dll'}

uses 
    System,
    FormsABC,
    LousySudoku;

const
    windowTitle : string = 'LousySudoku v1';
var
    theSudoku : Sudoku;
procedure FileChooseWindow();
begin
    MainForm.IsFixedSize := true;
    MainForm.Height := 150;
    MainForm.Width := 400;    
    var buttonOk : Button := new Button('Generate sudoku');        
end;

begin
    MainForm.Title := windowTitle;
    FileChooseWindow();
end.