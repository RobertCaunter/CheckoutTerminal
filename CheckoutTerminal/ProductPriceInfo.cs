namespace CheckoutTerminal {
	public class ProductPriceInfo : IProductPriceInfo {
		public ProductPriceInfo(string productCode, decimal pricePerUnit, int? volumeAmount = null, decimal? volumePrice = null) {
			ProductCode = productCode;
			PricePerUnit = pricePerUnit;
			Volume = volumeAmount;
			VolumePrice = volumePrice;
		}

		public decimal PricePerUnit { get; set; }

		public string ProductCode { get; set; } = string.Empty;

		public int? Volume { get; set; }

		public decimal? VolumePrice { get; set; }

		public decimal GetPriceForProduct(int productCount) {
			decimal price = 0;
			int remainingProduct = productCount;

			if (Volume.HasValue && VolumePrice.HasValue) {
				int volumeCount = (remainingProduct / Volume.Value);
				remainingProduct -= volumeCount * Volume.Value;
				price += volumeCount * VolumePrice.Value;
			}

			price += remainingProduct * PricePerUnit;
			return price;
		}
	}
}
