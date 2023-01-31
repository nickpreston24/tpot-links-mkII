# Why is this here?

The [Embedded File Provider](/Embeds/EmbeddedFileProvider.cs) is some middleware or support that I found which will let us read files **locally** relative to the Assembly they're in.

In other words, we can finally read things like `*.sql` files right next to the UI Components we want, be they Views, Pages, `*.jsx`, `*.vue` or whatever.  Personally, I think it goes great with [HTMX](htmx.org), as you can fire off a long SQL stored Procedure and HTMX will happily wait for the HTML result you render back to the calling DOM Element, then swapps the result straight into the DOM (magic!)