
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
namespace XGUI
{
	/// <summary>
	/// A horizontal slider with a text entry on the right
	/// </summary>
	public class SliderScaleEntry : Panel
	{
		public SliderScale Slider { get; protected set; }
		public TextEntry TextEntry { get; protected set; }

		public float MinValue
		{
			get => Slider.MinValue;
			set
			{
				Slider.MinValue = value;
				TextEntry.MinValue = value;
			}
		}

		public float MaxValue
		{
			get => Slider.MaxValue;
			set
			{
				Slider.MaxValue = value;
				TextEntry.MaxValue = value;
			}
		}

		public float Step
		{
			get => Slider.Step;
			set => Slider.Step = value;
		}

		public string Format
		{
			get => TextEntry.NumberFormat;
			set => TextEntry.NumberFormat = value;
		}


		public SliderScaleEntry()
		{
			AddClass( "sliderentry" );

			Slider = AddChild<SliderScale>();
			TextEntry = Slider.SliderArea.AddChild<TextEntry>();
			TextEntry.Numeric = true;
			TextEntry.NumberFormat = "0.###";

			Slider.AddEventListener( "value.changed", () => onSliderChanged( Slider.Value ) );
			TextEntry.AddEventListener( "value.changed", () => onEntryChanged( TextEntry.Value ) );
		}
		void onEntryChanged( string value )
		{
			Slider.Value = value.ToFloat();
			OnValueChanged( value );
		}
		void onSliderChanged( float value )
		{
			TextEntry.Value = value.ToString();
			OnValueChanged( value );
		}
		protected void OnValueChanged( object value )
		{
			CreateValueEvent( "value", value );
		}

		/// <summary>
		/// The actual value. Setting the value will snap and clamp it.
		/// </summary>
		public float Value
		{
			get => Slider.Value;
			set => Slider.Value = value;
		}

		public override void SetProperty( string name, string value )
		{
			if ( name == "min" || name == "max" || name == "value" || name == "step" || name == "mintext" || name == "maxtext" || name == "label" )
			{
				Slider.SetProperty( name, value );
				return;
			}

			if ( name == "format" )
			{
				TextEntry.NumberFormat = value;
				return;
			}
			base.SetProperty( name, value );
		}
	}

	namespace Construct
	{
		public static class SliderScaleWithEntryConstructor
		{
			public static SliderScaleEntry SliderScaleWithEntry( this PanelCreator self, float min, float max, float step, string mintext, string maxtext )
			{
				var control = self.panel.AddChild<SliderScaleEntry>();
				control.MinValue = min;
				control.MaxValue = max;
				control.Step = step;

				return control;
			}
		}
	}
}
