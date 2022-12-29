mergeInto(LibraryManager.library, {

  JoinChannel: async function() {
    await joinChannel();
  },

  ChangeMuteState: async function(muteBool){
    await changeMuteState(muteBool);
  },

  FetchTokenInternal: async function() {
    await fetchToken();
  },
  
});