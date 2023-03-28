namespace CheckoutTerminal {
	public class MissingProductPriceException : Exception {
		public MissingProductPriceException(string message) : base(message) { }
	}
}
