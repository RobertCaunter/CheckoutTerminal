namespace CheckoutTerminal {
	public interface IProductPriceInfo {
		string ProductCode { get; set; }

		decimal GetPriceForProduct(int count);
	}
}
