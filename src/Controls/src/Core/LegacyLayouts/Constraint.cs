#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.Maui.Controls.Internals;

#pragma warning disable CS0618, CS0619 // Type or member is obsolete

namespace Microsoft.Maui.Controls.Compatibility
{
	[System.ComponentModel.TypeConverter(typeof(ConstraintTypeConverter))]
	public class Constraint
	{
		Func<RelativeLayout, double> _measureFunc;

		public Constraint()
		{
		}

		internal IEnumerable<View> RelativeTo { get; set; }

		public static Constraint Constant(double size)
		{
			var result = new Constraint { _measureFunc = parent => size };

			return result;
		}

		public static Constraint FromExpression(Expression<Func<double>> expression)
		{
			Func<double> compiled = expression.Compile();
			var result = new Constraint
			{
				_measureFunc = layout => compiled(),
				RelativeTo = ExpressionSearch.Default.FindObjects<View>(expression).ToArray() // make sure we have our own copy
			};

			return result;
		}

		public static Constraint RelativeToParent(Func<RelativeLayout, double> measure)
		{
			var result = new Constraint { _measureFunc = measure };

			return result;
		}

		public static Constraint RelativeToView(View view, Func<RelativeLayout, View, double> measure)
		{
			var result = new Constraint { _measureFunc = layout => measure(layout, view), RelativeTo = new[] { view } };

			return result;
		}

		internal double Compute(RelativeLayout parent)
		{
			return _measureFunc(parent);
		}
	}
}

#pragma warning restore CS0618, CS0619 // Type or member is obsolete