{$reference 'core.dll'}

uses 
    System,
    FormsABC,
    LousySudoku;

const
    windowTitle : string = 'LousySudoku v1';
var
    theSudoku : Sudoku;
procedure ClickOk();
begin
end;
function PutTextBox(boxLabel : string; boxText : string; boxHeight : integer; boxWidth : integer) : TextBox;
begin
    var thLabel : TextLabel := new TextLabel(boxLabel);
    var textBox : TextBox := new TextBox();
    textBox.Text := boxText;
    textBox.Height := boxHeight;
    textBox.Width := boxWidth;
    Result := textBox;
end;

procedure OpenFile(obj : object);
begin
    
end;

procedure FileChooseWindow();
begin
    MainForm.IsFixedSize := true;
    MainForm.Height := 150;
    MainForm.Width := 400;    
    filenameTextBox := PutTextBox(
            'Template filename: ', 
            directory + extension,
            24,
            270
        );
    complexityTextBox := PutTextBox(
            'Complexity [0-1]: ', 
            complexityDefault.ToString(),
            24,
            50
        );
    var buttonOk : Button := new Button('Generate sudoku');        
    buttonOk.Click += ClickOk;
end;

begin
    MainForm.Title := windowTitle;
    FileChooseWindow();
end.