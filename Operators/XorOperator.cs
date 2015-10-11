namespace HiSystems.Interpreter {
	/// <summary>
	/// Logical Xor operator.
	/// Usage: booleanValue XOR booleanValue
	/// Example: true XOR false
	/// </summary>
	public class XorOperator : Operator
	{
		public XorOperator()
		{
		}

		/// <summary>
		/// Non-zero arguments are considered true.
		/// </summary>
		internal override Literal Execute(IConstruct argument1, IConstruct argument2)
		{
			return base.GetTransformedConstruct<Boolean>(argument1) ^ base.GetTransformedConstruct<Boolean>(argument2);
		}

		public override string Token
		{
			get
			{
				return "XOR";
			}
		}
	}
}