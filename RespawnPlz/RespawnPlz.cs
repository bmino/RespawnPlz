using BepInEx;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

namespace RespawnPlz {
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.bmino.RespawnPlz", "RespawnPlz", "1.0.0")]
    public class RespawnPlz : BaseUnityPlugin {
        public void Awake() {
            Logger.LogMessage("Loaded RespawnPlz!");

            On.RoR2.Console.RunCmd += (On.RoR2.Console.orig_RunCmd orig, RoR2.Console self, RoR2.Console.CmdSender sender, string concommandName, List<string> userArgs) => {
                if (concommandName == "say" && userArgs.FirstOrDefault() == "respawnplz") {
                    if (!isHost()) {
                        Logger.LogMessage("Skipping respawn logic on non-host");
                        return;
                    }
                    if (isDead(sender.networkUser)) {
                        Logger.LogMessage("Cannot respawn a dead guy");
                        return;
                    }
                    CharacterMaster master = sender.networkUser.master;
                    ChatHelper.sendBroadcastChat($"That idiot, {sender.networkUser.userName}, is stuck again...");
                    Stage.instance.RespawnCharacter(master); 
                } else {
                    orig(self, sender, concommandName, userArgs);
                }
            };
        }

        public bool isHost() {
            return NetworkServer.active && Stage.instance != null;
        }

        public bool isDead(NetworkUser player) {
            return !player.master.hasBody;
        }
    }
}