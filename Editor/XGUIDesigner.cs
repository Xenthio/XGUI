using Editor;
using Editor.PanelInspector;
using System.Collections.Generic;
namespace XGUI;
public static class XGUIMenu
{
	[Menu( "Editor", "XGUI/Designer" )]
	public static void OpenMyMenu()
	{
		var b = new XGUIDesigner();
	}
}

public class XGUIDesigner : DockWindow
{/*
	private readonly UndoStack _undoStack = new();
	private Option _undoOption;
	private Option _redoOption;

	private Option _undoMenuOption;
	private Option _redoMenuOption;*/

	private Menu _recentFilesMenu;
	private readonly List<string> _recentFiles = new();

	XGUIView _view;
	PanelInspectorWidget _inspector;
	Widget _widget;
	public XGUIDesigner()
	{
		DeleteOnClose = true;

		Title = "XGUI Window Designer";
		Size = new Vector2( 1280, 720 );

		//_graph = new();

		//CreateToolBar();

		//_recentFiles = FileSystem.Temporary.ReadJsonOrDefault( "xguiwindowdesigner_recentfiles.json", _recentFiles )
		//	.Where( x => System.IO.File.Exists( x ) ).ToList();

		CreateUI();
		Show();

		//CreateNew();
	}
	protected override bool OnClose()
	{
		_view.Destroy();
		return base.OnClose();
	}
	public void CreateUI()
	{
		BuildMenuBar();

		_view = new XGUIView();
		_view.SetSizeMode( SizeMode.Expand, SizeMode.Expand );
		var b = new Widget();
		_inspector = new PanelInspectorWidget( b );
		this.DockManager.AddDock( null, _view );
		this.DockManager.AddDock( null, b, split: 0.21f );

	}
	public void BuildMenuBar()
	{
		var file = MenuBar.AddMenu( "File" );
		file.AddOption( "New", "common/new.png", New, "Ctrl+N" ).StatusText = "New Graph";
		file.AddOption( "Open", "common/open.png", Open, "Ctrl+O" ).StatusText = "Open Graph";
		file.AddOption( "Save", "common/save.png", () => Save(), "Ctrl+S" ).StatusText = "Save Graph";
		file.AddOption( "Save As...", "common/save.png", () => Save( true ), "Ctrl+Shift+S" ).StatusText = "Save Graph As...";

		file.AddSeparator();

		_recentFilesMenu = file.AddMenu( "Recent Files" );

		file.AddSeparator();

		file.AddOption( "Quit", null, Close, "Ctrl+Q" ).StatusText = "Quit";

		var edit = MenuBar.AddMenu( "Edit" );/*
		_undoMenuOption = edit.AddOption( "Undo", "undo", Undo, "Ctrl+Z" );
		_redoMenuOption = edit.AddOption( "Redo", "redo", Redo, "Ctrl+Y" );
		_undoMenuOption.Enabled = _undoStack.CanUndo;
		_redoMenuOption.Enabled = _undoStack.CanRedo;*/

		edit.AddSeparator();
		edit.AddOption( "Cut", "common/cut.png", CutSelection, "Ctrl+X" );
		edit.AddOption( "Copy", "common/copy.png", CopySelection, "Ctrl+C" );
		edit.AddOption( "Paste", "common/paste.png", PasteSelection, "Ctrl+V" );
		edit.AddOption( "Select All", "select_all", SelectAll, "Ctrl+A" );
	}

	void New()
	{

	}
	void Open()
	{

	}
	void Save( bool saveas = false )
	{

	}

	void CutSelection()
	{

	}
	void CopySelection()
	{

	}
	void PasteSelection()
	{

	}
	void SelectAll()
	{

	}
}
