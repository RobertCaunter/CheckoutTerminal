using CheckoutTerminal;
using Moq;

namespace CheckoutTerminalUnitTests {
	public class DictionaryPricingModelTests {
		private Dictionary<string, IProductPriceInfo> _pricingData;

		[SetUp]
		public void Setup() {
			_pricingData = new Dictionary<string, IProductPriceInfo>();
		}

		[Test]
		public void PricingModelContainsProductPriceInfo() {
			_pricingData.Add("A", new Mock<IProductPriceInfo>().Object);
			var pricingModel = new DictionaryPricingModel(_pricingData);

			Assert.That(pricingModel.HasPriceDataForProduct("A"), "The pricing model should contain price info to the product 'A'.");
		}

		[Test]
		public void PricingModelDoesNotContainProductPriceInfo() {
			var pricingModel = new DictionaryPricingModel(_pricingData);

			Assert.That(!pricingModel.HasPriceDataForProduct("A"), "The pricing model should not contain price info to the product 'A'.");
		}

		[Test]
		public void PricingModelCalculatesPriceCorrectly() {
			var priceInfo = new Mock<IProductPriceInfo>();
			priceInfo.Setup(p => p.GetPriceForProduct(It.IsAny<int>())).Returns(1.25m);
			priceInfo.SetupGet(p => p.ProductCode).Returns("A");
			_pricingData.Add("A", priceInfo.Object);
			var pricingModel = new DictionaryPricingModel(_pricingData);
			Assert.That(pricingModel.GetPrice("A", 1), Is.EqualTo(1.25m), "The pricing model is not calculating the price correctly.");
		}

		[Test]
		[TestCase("A", 1.25)]
		[TestCase("B", 4.25)]
		public void PricingModelGetsTheCorrectProductPrice(string productCode, decimal expectedPrice) {
			var aPriceInfo = new Mock<IProductPriceInfo>();
			aPriceInfo.Setup(p => p.GetPriceForProduct(It.IsAny<int>())).Returns(1.25m);
			aPriceInfo.SetupGet(p => p.ProductCode).Returns("A");
			var bPriceInfo = new Mock<IProductPriceInfo>();
			bPriceInfo.Setup(p => p.GetPriceForProduct(It.IsAny<int>())).Returns(4.25m);
			bPriceInfo.SetupGet(p => p.ProductCode).Returns("B");
			_pricingData.Add("A", aPriceInfo.Object);
			_pricingData.Add("B", bPriceInfo.Object);
			var pricingModel = new DictionaryPricingModel(_pricingData);
			Assert.That(pricingModel.GetPrice(productCode, 1), Is.EqualTo(expectedPrice), $"The pricing model is not getting the correct price for product {productCode}.");
		}

		[Test]
		[TestCase(1, 1.25)]
		[TestCase(2, 2.50)]
		[TestCase(3, 3.75)]
		public void PricingModelUsesTheProductCount(int count, decimal expectedPrice) {
			var aPriceInfo = new Mock<IProductPriceInfo>();
			aPriceInfo.Setup(p => p.GetPriceForProduct(It.Is<int>(c => c == 1))).Returns(1.25m);
			aPriceInfo.Setup(p => p.GetPriceForProduct(It.Is<int>(c => c == 2))).Returns(2.50m);
			aPriceInfo.Setup(p => p.GetPriceForProduct(It.Is<int>(c => c == 3))).Returns(3.75m);
			aPriceInfo.SetupGet(p => p.ProductCode).Returns("A");
			_pricingData.Add("A", aPriceInfo.Object);
			var pricingModel = new DictionaryPricingModel(_pricingData);
			Assert.That(pricingModel.GetPrice("A", count), Is.EqualTo(expectedPrice), $"The pricing model is not getting the correct price for {count} of the product.");
		}

		[Test]
		public void PricingModelThrowsExceptionWhenPricingInfoDoesNotExist() {
			var pricingModel = new DictionaryPricingModel(_pricingData);
			Assert.Throws<MissingProductPriceException>(() => pricingModel.GetPrice("A", 1), "Expected an exception due to a missing product price.");
		}

		[Test]
		public void PricingModelAddsProductPriceInfo() {
			var pricingModel = new DictionaryPricingModel(_pricingData);
			Assert.That(!pricingModel.HasPriceDataForProduct("A"), "The pricing model should not contain price info to the product 'A'.");

			var priceInfo = new Mock<IProductPriceInfo>();
			priceInfo.SetupGet(p => p.ProductCode).Returns("A");
			Assert.That(pricingModel.AddProductPriceInfo(priceInfo.Object), "Adding the product info should return true to indicate success.");

			Assert.That(pricingModel.HasPriceDataForProduct("A"), "The pricing model should now contain price info to the product 'A'.");
		}

		[Test]
		public void DoesNotUpdatePriceInfoWhenOverrideIsFalse() {
			var aPriceInfo = new Mock<IProductPriceInfo>();
			aPriceInfo.Setup(p => p.GetPriceForProduct(It.IsAny<int>())).Returns(1.25m);
			aPriceInfo.SetupGet(p => p.ProductCode).Returns("A");
			_pricingData.Add("A", aPriceInfo.Object);
			var pricingModel = new DictionaryPricingModel(_pricingData);

			var bPriceInfo = new Mock<IProductPriceInfo>();
			bPriceInfo.Setup(p => p.GetPriceForProduct(It.IsAny<int>())).Returns(4.25m);
			bPriceInfo.SetupGet(p => p.ProductCode).Returns("A");
			Assert.Multiple(() => {
				Assert.That(!pricingModel.AddProductPriceInfo(bPriceInfo.Object, false), "Adding the product info should return false and not update the price Info.");
				Assert.That(pricingModel.GetPrice("A", 1), Is.EqualTo(1.25m), "The price should still be the old price of 1.25m and not updated to 4.25m.");
			});
		}

		[Test]
		public void DoesAddNewPriceInfoEvenIfOverrideIsFalse() {
			var pricingModel = new DictionaryPricingModel(_pricingData);

			var PriceInfo = new Mock<IProductPriceInfo>();
			PriceInfo.Setup(p => p.GetPriceForProduct(It.IsAny<int>())).Returns(4.25m);
			PriceInfo.SetupGet(p => p.ProductCode).Returns("A");
			Assert.Multiple(() => {
				Assert.That(pricingModel.AddProductPriceInfo(PriceInfo.Object, false), "Adding the product info should return true and add the new price Info.");
				Assert.That(pricingModel.HasPriceDataForProduct("A"), "The pricing model should now contain price info to the product 'A'.");
			});
		}
	}
}
