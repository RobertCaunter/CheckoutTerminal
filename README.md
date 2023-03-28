# CheckoutTerminal
This is a simple C# library project (with tests) done for a techinal exercise for a job application.

The project was to make a very simple set of classes to simulated a check out terminal at a supermarket.
The project has three main components:
1. The terminal - keeps track of the objects scans and computes their current cost.
      * I created an interface for the main functionality of the terminal to enable easy unit testing with integrating applications.
      * The class that implements the interface (PointOfSaleTerminal) throws exceptions if the price is missing for a product or if the product code is invalid.
     * For the product code, I decided not to make it case insensitive as I assumed it was not meant to be a Human readable code and therefore honouring case means that there are more potential codes available.
      * I also added a method to check if the product code is valid. Right now, all it does it check for an empty string, but it could be extended if product codes were expected to follow a specific format.
2. The pricing model - holds all the price information. This would likely be a singleton, as the data shouldn't change between sessions.
      * I created another interface for this class. Mostly to allow different methods of obtaining the pricing data. It could potentially connect to a database or use an xml/json file. The implementing class that I created just uses a dictionary as it was easiest for this task and for testing.
      * I added a method for updating the price info of the pricing model, this could be a way of updating a database or for just allowing a clerk to override the price of a product if the class was not a singleton but rather read in the price data at the start of the transaction session.
3. The individual product price data - this is just the price data and calculation for each individual product.
     * Making this class also have an interface might be a slight case of over-engineering, but it does make it easier to unit test the other classes. It also allows  products to have different methods for calculating their price.
