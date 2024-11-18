using Sandbox.UI;

namespace XGUI;

public class ListOption : Panel
{
	private bool _selected;

	public bool Selected
	{
		get => _selected;
		set
		{
			SetClass( "selected", value );
			_selected = value;
		}
	}
	public ListOption()
	{
		SetClass( "listoption", true );
	}
	public SelectList ParentList;
	protected override void OnClick( MousePanelEvent e )
	{
		base.OnClick( e );
		if ( ParentList == null )
		{
			Log.Error( "ListOption used outside of SelectList" );
			return;
		}

		ParentList.OptionSelected( this );
	}
}
