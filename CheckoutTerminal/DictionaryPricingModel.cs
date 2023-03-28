namespace CheckoutTerminal {
	public class DictionaryPricingModel : IPricingModel {
		Dictionary<string, IProductPriceInfo> _productPrices;
		public DictionaryPricingModel(Dictionary<string, IProductPriceInfo> productPrices) {
			_productPrices= productPrices;
		}

		public decimal GetPrice(string productCode, int count) {
			if (_productPrices.ContainsKey(productCode)) {
				return _productPrices[productCode].GetPriceForProduct(count);
			}
			throw new MissingProductPriceException($"Missing price data for the product code: {productCode}.");
		}

		public bool AddProductPriceInfo(IProductPriceInfo productPriceInfo, bool overrideIfPresent = true) {
			if (overrideIfPresent || !_productPrices.ContainsKey(productPriceInfo.ProductCode)) {
				_productPrices.Add(productPriceInfo.ProductCode, productPriceInfo);
				return true;
			}
			return false;
		}

		public bool HasPriceDataForProduct(string productCode) {
			return _productPrices.ContainsKey(productCode);
		}
	}
}
