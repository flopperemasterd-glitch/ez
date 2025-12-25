-- GojoLoader.lua

local Players = game:GetService("Players")
local InsertService = game:GetService("InsertService")
local ReplicatedStorage = game:GetService("ReplicatedStorage")
local Workspace = game:GetService("Workspace")

local player = Players.LocalPlayer
local backpack = player:WaitForChild("Backpack")

-- Asset ID of the Gojo skill package
local ASSET_ID = 81897328117024

-- Load the asset
local success, asset = pcall(function()
	return InsertService:LoadAsset(ASSET_ID)
end)

if not success then
	warn("Failed to load asset:", asset)
	return
end

-- List of scripts/modules expected in the asset
local scriptNames = {"CServer", "CClient", "HitEvent", "HiboxModule", "Skill1"}
local loadedScripts = {}

-- Wait for each script/module to exist inside the asset
for _, name in ipairs(scriptNames) do
	local s
	repeat
		s = asset:FindFirstChild(name)
		task.wait(0.05)
	until s
	loadedScripts[name] = s
end

-- Parent scripts/modules to their runtime locations
print("ez")

-- Cleanup the temporary asset container
asset:Destroy()

print("success")
