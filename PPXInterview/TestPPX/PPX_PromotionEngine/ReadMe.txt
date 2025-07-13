PromotionEngine is an API that receives a list of items 
and returns for each item its new price (after discounts calculation).

The promotionEngine currently uses two 3rd parties providers (external reference as API) - Visa and Loyalty.
Each one of the promotion providers gets the item with its original price and returns the discount.

Each one of the providers expose two methods :

	1. GetDiscountableItemIds - returns the ids of items that have discount (each call will return the same ids)
	2. GetItemDiscount - Gets the item and its original price and returns its discount.
	

The catalog of items that are supported is from ids 1-13.

1. Discount for item can be calcuated by one provider/all providers/none of them
   The item total discount is the sum of all the calculated discounts  
2. In case of failure/connection issue/any error from the provider = no discount on the item.
3. Items with the same id will get the same discount from the provider
4. The new price returned value should be original price - total item discounts (in case of no discount it should be the original price)

What are our expectations from you?

*	There are few bugs + performance issues that should be fixed (by the assumptions above)
*	This code should be ready for production by all it means
*	This code should be extensible so it will allow us easily to connect to more providers 
	In future (without changing much of the logic)

	

	Good Luck! :)