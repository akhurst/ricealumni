$ ->

  SelectItem = (item,lens) ->
    item.addClass("selected").siblings().removeClass("selected")

  ActivateItem = (item,lens) ->
    switch lens.context
      when "Socket"
        id = item.attr("data-content-item-id")
        resultsField = $("#" + lens.hideId)
        resultsField.attr("value", if resultsField.attr("value") then resultsField.attr("value") + "," + id else id)

        # TODO: Fire off an AJAX event to generate a new connector editor
        $.ajax(lens.socketLoc + "&newRightId=" + idsearch + lens.params, {
          success: (data) ->
            # Find the socket contents

            lens.child.html(data)
            BindResults(lens)
          })

      when "Search"
        # Redirect
        # url = item.attr("data-lens-view-url")
        url = item.find("header a").attr "href"
        document.location = url

  BindResults = (lens) ->
    if lens.child.children().length
      lens.child.addClass("has-results")
    else
      lens.child.removeClass("has-results")
    lens.child.children().click ->
      ActivateItem($(this),lens)

  Parameterise = (a) -> 
    paramName = a.name.substring "data-lens-param-".length
    # To camel case: http://stackoverflow.com/questions/6660977/convert-hyphens-to-camel-case-camelcase
    camelCased = paramName.replace(/-([a-z])/g, (c) -> c[1].toUpperCase())
    "&" + camelCased + "=" + a.value

    # .not("[data-lens-bound]").attr("data-lens-bound", true)
  BindLens = () ->
    lens = {
      textId: $(this).attr("data-lens-text-id")
      hideId: $(this).attr("data-lens-hidden-id")
      resultsId: $(this).attr("data-lens-results-id")
      sendLoc: $(this).attr("data-lens-url")
      context: $(this).attr("data-lens-param-context")
      # Build params string from data attributes
      params: (Parameterise attrib for attrib,i in this.attributes when attrib.name.match(/^data-lens-param-/gi)).join()
    }
    lens.text = $("#" + lens.textId)
    lens.child = $("#" + lens.resultsId)
    if (lens.context=="Socket")
      lens.socket = $(this).parents(".socket").first()
      lens.socketChild = lens.socket.children(".zone-content").children(".socket-content")
      lens.socketLoc = $(this).attr("data-lens-socket-url");

    performSearch = (me) ->
      search = me.value;
      lens.child.show()
      $.ajax(lens.sendLoc + "query=" + search + lens.params, {
        success: (data) ->
          lens.child.html(data)
          BindResults(lens)
        })

    switch lens.context
      when "Socket", "Search"
        lens.text.focus( () -> 
          if (lens.child.hasClass("has-results"))
            lens.child.show()
        ).blur( () ->
          resultsHideFunc = () -> lens.child.hide()
          setTimeout(resultsHideFunc,200)
        )

    # Timeouts for typing so we don't *immediately* search with every keypress 
    timeoutId = null;
    lens.text.keydown (ev) ->
      selection = lens.child.children(".selected")
      switch ev.which
        # Return (actually select the item)
        when 13
          if selection.length
            ev.preventDefault() # Prevent default search
            ActivateItem(selection,lens)

        # Up arrow
        when 38
          if (!selection.length)
            SelectItem(lens.child.children().last(),lens)
          else
            prev = selection.prev()
            SelectItem((if prev.length then prev else lens.child.children().last()),lens)

        # Down arrow
        when 40
          if (!selection.length)
            SelectItem(lens.child.children().first(),lens)
          else
            next = selection.next()
            SelectItem((if next.length then next else lens.child.children().first()),lens)
        
        else  # All other keys (entering a search term)
          if (timeoutId != null)
            clearTimeout(timeoutId)
            timeoutId = null
            
          timeoutFunc = => performSearch(this)
          timeoutId = setTimeout(timeoutFunc, 300)

  $("[data-lens]").each(BindLens)