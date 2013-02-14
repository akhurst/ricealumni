Delta = @Delta = @Delta ? {}

###
namespace Delta.Text
  Text and language processing functions
###
Delta.Text = Delta.Text ? {}

###
class HtmlTextWriter 
  Converts text into HTML formats
###
Delta.Text.HtmlTextPrinter = class HtmlTextPrinter extends Delta.Html.ElementEntity
  
  Markdown: ()->
    if !@_Markdown?
      @_Markdown = window.markdown
    @_Markdown

  onPrint: (print)->
    # TODO: Might be cleaner to process print types in the events model
    html = 
      switch print.Type
        when "Markdown"
          @Markdown().toHTML(print.Content)
        when "HTML"
          print.Content

    filter = 
      Nodes: $(html)
      Cancel: false

    @Raise("HtmlInsertPreProcess", filter)
    if !filter.Cancel
      @$.append(filter.Nodes)
      @Raise("HtmlInsertPostProcess", filter)

  onMessage: (message)->
    # Handle messages somehow

###
namespace Delta.Entities
class BaseEntity
  method Text
    Extend base Entity with text processing
###
Delta.Entities.BaseEntity::Text = (thing,context={},qualifier)->
  # Calling entity should always be the subject
  if !context.Subject? then context.Subject = @
  if qualifier? then context.Qualifier = qualifier
  result = null
  if qualifier?
    result = @_MakeText("text"+thing+"_"+qualifier,context)
  if !result?
    result = @_MakeText("text"+thing, context)
  if !result?
    result = @Parent?.Text(thing,context,qualifier)
  result

Delta.Entities.BaseEntity::_MakeText = (name,context)->
  target = @[name]
  if !target? then return null
  # TODO: Need to handle various other types ... e.g. arrays of weighted strings ... could also perform some prototype traversal to
  # help getting alternates
  if typeof target is "string"
    target
  else
    target.call(@,context)

Delta.Entities.BaseEntity::ProcessText = (thing,context,qualifier)->

Delta.Entities.BaseEntity::Write = (thing,context = {},qualifier)->
  result = @Text(thing,context,qualifier)

  context.Content = result ? "Text not found: '#{thing}' on '#{ if thing == "Name" then @.toString() else @Write("Name", context)}'"
  context.Type = "Markdown"
  
  @Raise("Print", context)
    
# Solve a problem: if we #{@context.Target} we'll get the normal name of that entity...    
Delta.Entities.BaseEntity::toString = (context)->
  # Optional context
  @Text("Name",context)

# Default name writer
Delta.Entities.BaseEntity::textName = (o)->
  # Write class name and #Id
  @constructor.name + " " + (@Name ? ("#" + @Id))
