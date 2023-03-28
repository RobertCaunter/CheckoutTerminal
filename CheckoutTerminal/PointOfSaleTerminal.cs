namespace CheckoutTerminal {
	public class PointOfSaleTerminal : ICheckoutTerminal {
		private IPricingModel _pricingModel;
		private Dictionary<string, int> _shoppingBasket = new Dictionary<string, int>();

		public PointOfSaleTerminal(IPricingModel pricingModel) {
			_pricingModel = pricingModel;
		}

		public decimal CalculateTotal() {
			decimal totalPrice = 0;
			foreach (var product in _shoppingBasket) {
				totalPrice += _pricingModel.GetPrice(product.Key, product.Value);
			}
			return totalPrice;
		}

		public int GetProductCount(string productCode) {
			return _shoppingBasket[productCode];
		}

		public void ScanProduct(string productCode) {
			if (!IsValidProductCode(productCode)) {
				throw new InvalidProductException($"{productCode} is not a valid product code.");
			}

			if (!_pricingModel.HasPriceDataForProduct(productCode)) {
				throw new MissingProductPriceException($"Missing price data for the product code: {productCode}.");
			}

			if (!_shoppingBasket.ContainsKey(productCode)) {
				_shoppingBasket[productCode] = 0;
			}

			_shoppingBasket[productCode]++;
		}

		public void SetPricing(IPricingModel pricingModel) {
			_pricingModel= pricingModel;
		}

		private bool IsValidProductCode(string productCode) {
			return !string.IsNullOrEmpty(productCode);
		}
	}
}