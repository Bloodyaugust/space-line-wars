startedAttackResearch = false
startedTierIResearch = false
startedTierIIResearch = false
swappedToCorvetteProduction = false
swappedToMissileProduction = false

function update ()
  if AliveTime > 10 and startedAttackResearch == false then
    BaseNode.CurrentResearch = Researches[1]
    startedAttackResearch = true
  end

  if ResourceRate >= 3 and startedTierIResearch == false and BaseNode.CurrentResearch == nil then
    BaseNode.CurrentResearch = Researches[2]
    startedTierIResearch = true
  end

  if startedTierIResearch == true and BaseNode.CurrentResearch == nil and swappedToCorvetteProduction == false then
    ProductionNodes[1].CurrentShip = ProductionNodes[1].ShipDataset[2]
    swappedToCorvetteProduction = true
  end

  if ResourceRate >= 5 and startedTierIIResearch == false and BaseNode.CurrentResearch == nil then
    BaseNode.CurrentResearch = Researches[3]
    startedTierIIResearch = true
  end

  if startedTierIIResearch == true and BaseNode.CurrentResearch == nil and swappedToMissileProduction == false then
    ProductionNodes[2].CurrentShip = ProductionNodes[2].ShipDataset[3]
    swappedToMissileProduction = true
  end
end
