mergeInto(LibraryManager.library, {

  BuyNFT: async function(tokenId) {
    return await createMarketSale(UTF8ToString(tokenId));
  },

  RequestFetchItemsListed: async function(storeAddress){
    await fetchItemsListed(UTF8ToString(storeAddress));
  },
  
  GetEmailPassword: async function(){
    await GetEmailPassword();
  },

  BuyWithTon: async function(){
    await BuyWithTon();
  },

});