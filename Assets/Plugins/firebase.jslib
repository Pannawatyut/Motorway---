mergeInto(LibraryManager.library, {
    GetJSON: function(path, objectName, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);
        
        try {
            firebase.database().ref(parsedPath).once('value').then(function(snapshot) {
                // Send the JSON string or some result back to Unity
                UnityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot.val()));
            }).catch(function(error) {
                // Handle the error case by sending a fallback message to Unity
                UnityInstance.SendMessage(parsedObjectName, parsedFallback, error.message);
            });
        } catch (error) {
            // Handle any unexpected errors
            UnityInstance.SendMessage(parsedObjectName, parsedFallback, error.message);
        }
    }
});
