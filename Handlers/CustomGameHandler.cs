using Il2CppSG.Airlock;
using Il2CppSG.Airlock.Roles;
using UnityEngine;

namespace AirlockAPI.Handlers
{
    public class CustomGameHandler : MonoBehaviour
    {
        public static CustomGameHandler Current;
        public Dictionary<PlayerState, GameRole> AssignedRoles = new Dictionary<PlayerState, GameRole>();
        public GameStateManager State;
        public RoleManager Role;
        public VoteManager Vote;

        void Awake()
        {
            if (Current == null)
            {
                Current = this;
                Current.State = FindObjectOfType<GameStateManager>();
                Current.Role = FindObjectOfType<RoleManager>();
                Current.Vote = FindObjectOfType<VoteManager>();
            }
            else
            {
                Destroy(this);
            }
        }

        public virtual bool OnTargetedAction(ref PlayerState killer, ref PlayerState victim, ref int action) { return true; }
        public virtual bool OnPlayerVoted(ref PlayerState voter, ref PlayerState voted) { return true; }
        public virtual bool OnPlayerVotedSkip(ref PlayerState voter) { return true; }
        public virtual bool OnPlayerEjected(ref PlayerState ejectedPlayer, ref GameRole role) { return true; }
        public virtual bool OnMeetingCalled(ref PlayerState reportingPlayer) { return true; }
        public virtual bool OnBodyReported(ref PlayerState bodyReported, ref PlayerState reportingPlayer) { return true; }
        public virtual bool OnVotingBegan(ref PlayerState bodyReported, ref PlayerState reportingPlayer) { return true; }
        public virtual bool OnAllVotesCast() { return true; }
        public virtual bool OnGameStart() { return true; }
        public virtual bool OnBeforeAssignRoles() { return true; }
        public virtual void OnAfterAssignRoles() { return; }
        public virtual bool OnGameEnd(ref GameTeam teamThatWon) { return true; }

        public GameRole GetTrueRole(PlayerState player)
        {
            foreach (Il2CppSystem.Collections.Generic.KeyValuePair<GameRole, Il2CppSystem.Collections.Generic.List<int>> roleEntry in Current.Role.gameRoleToPlayerIds)
            {
                foreach (int id in roleEntry.Value)
                {
                    if (player.IsConnected)
                    {
                        if (id == player.PlayerId)
                        {
                            return roleEntry.Key;
                        }
                    }
                }
            }

            return GameRole.NotSet;
        }
    }
}
