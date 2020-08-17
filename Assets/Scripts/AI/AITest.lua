startedResearch = false

function update ()
  if AliveTime > 10 and startedResearch == false then
    BaseNode.CurrentResearch = Researches[1]
    startedResearch = true
  end
end
