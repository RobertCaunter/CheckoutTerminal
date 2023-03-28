namespace CheckoutTerminal {
	public interface ICheckoutTerminal {
		void SetPricing(IPricingModel pricingModel);

		void ScanProduct(string productCode);

		decimal CalculateTotal();

		int GetProductCount(string productCode);
	}
}
