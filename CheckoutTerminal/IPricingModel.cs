namespace CheckoutTerminal {
	public interface IPricingModel {
		bool HasPriceDataForProduct(string productCode);

		decimal GetPrice(string productCode, int count);

		public bool AddProductPriceInfo(IProductPriceInfo productPriceInfo, bool overrideIfPresent = true);
	}
}
