mergeInto(LibraryManager.library, {
    Ad_Preroll: function(type, name)
    {
        adBreak({
            type : UTF8ToString(type),
            name : UTF8ToString(name),
            adBreakDone : (info) => 
            {
                console.log(info);
            },
        });
    },
})