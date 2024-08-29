mergeInto(LibraryManager.library, {
    GetJSON: function(path, objectName, callback, fallback) {
        var parsedPath = Pointer_stringify(path);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
        
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
