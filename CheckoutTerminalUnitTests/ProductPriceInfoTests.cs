using CheckoutTerminal;

namespace CheckoutTerminalUnitTests {
	public class ProductPriceInfoTests {

		[Test]
		[TestCase(1, 1.25)]
		[TestCase(2, 2.50)]
		[TestCase(3, 3.75)]
		[TestCase(4, 5.00)]
		public void CalculatesPriceCorrectly(int count, decimal expectedPrice) {
			var priceInfo = new ProductPriceInfo("A", 1.25m);
			Assert.That(priceInfo.GetPriceForProduct(count), Is.EqualTo(expectedPrice), $"Got an unexpected price for {count} of the product.");
		}

		[Test]
		[TestCase(1, 1.25)]
		[TestCase(2, 2.50)]
		[TestCase(3, 3.00)]
		[TestCase(4, 4.25)]
		[TestCase(5, 5.50)]
		[TestCase(6, 6.0)]
		[TestCase(7, 7.25)]
		public void UsesVolumePricesIfBothPriceAndAmountArePresent(int count, decimal expectedPrice) {
			var priceInfo = new ProductPriceInfo("A", 1.25m);
			priceInfo.Volume = 3;
			priceInfo.VolumePrice = 3.00m;
			Assert.That(priceInfo.GetPriceForProduct(count), Is.EqualTo(expectedPrice), $"Got an unexpected price for {count} of the product.");
		}

		[Test]
		[TestCase(1, 1.25)]
		[TestCase(2, 2.50)]
		[TestCase(3, 3.75)]
		[TestCase(4, 5.00)]
		public void DoesNotUseVolumePricesIfOnlyVolumeAmountIsPresent(int count, decimal expectedPrice) {
			var priceInfo = new ProductPriceInfo("A", 1.25m);
			priceInfo.Volume = 3;
			Assert.That(priceInfo.GetPriceForProduct(count), Is.EqualTo(expectedPrice), $"Got an unexpected price for {count} of the product.");
		}

		[Test]
		[TestCase(1, 1.25)]
		[TestCase(2, 2.50)]
		[TestCase(3, 3.75)]
		[TestCase(4, 5.00)]
		public void DoesNotUseVolumePricesIfOnlyVolumePriceIsPresent(int count, decimal expectedPrice) {
			var priceInfo = new ProductPriceInfo("A", 1.25m);
			priceInfo.VolumePrice = 3.00m;
			Assert.That(priceInfo.GetPriceForProduct(count), Is.EqualTo(expectedPrice), $"Got an unexpected price for {count} of the product.");
		}
	}
}
