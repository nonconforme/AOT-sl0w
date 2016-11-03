//Fixed With [DOGE]DEN aottg Sources fixer
//Doge Guardians FTW
//DEN is OP as fuck.
//Farewell Cowboy

using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PhotonPlayer
{
    private int actorID;
    public readonly bool isLocal;
    private string nameField;
    public object TagObject;
    public int ViolationCount;

    protected internal PhotonPlayer(bool isLocal, int actorID, Hashtable properties)
    {
        this.actorID = -1;
        this.nameField = string.Empty;
        this.customProperties = new Hashtable();
        this.isLocal = isLocal;
        this.actorID = actorID;
        this.InternalCacheProperties(properties);
        this.ViolationCount = 0;
    }

    public PhotonPlayer(bool isLocal, int actorID, string name)
    {
        this.actorID = -1;
        this.nameField = string.Empty;
        this.customProperties = new Hashtable();
        this.isLocal = isLocal;
        this.actorID = actorID;
        this.nameField = name;
        this.ViolationCount = 0;
    }

    public override bool Equals(object p)
    {
        PhotonPlayer player = p as PhotonPlayer;
        return ((player != null) && (this.GetHashCode() == player.GetHashCode()));
    }

    public static PhotonPlayer Find(int ID)
    {
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            PhotonPlayer player = PhotonNetwork.playerList[i];
            if (player.ID == ID)
            {
                return player;
            }
        }
        return null;
    }

    public static int Reconnectnum = 5;
    public void Reconnect(int i)
    {
        PhotonNetwork.networkingPeer.OpRaiseEvent(0xfe, null, true, null);
        Hashtable hashtable = new Hashtable();
        hashtable.Add(PhotonPlayerProperty.name, i.ToString());
        hashtable.Add(PhotonPlayerProperty.guildName, LoginFengKAI.player.guildname);
        hashtable.Add(PhotonPlayerProperty.kills, 0);
        hashtable.Add(PhotonPlayerProperty.max_dmg, 0);
        hashtable.Add(PhotonPlayerProperty.total_dmg, 0);
        hashtable.Add(PhotonPlayerProperty.deaths, 0);
        hashtable.Add(PhotonPlayerProperty.dead, true);
        hashtable.Add(PhotonPlayerProperty.isTitan, 0);
        hashtable.Add(PhotonPlayerProperty.RCteam, 0);
        hashtable.Add(PhotonPlayerProperty.currentLevel, string.Empty);
        PhotonNetwork.player.InternalChangeLocalID(Reconnectnum);
        PhotonNetwork.AllocateViewID();
        PhotonNetwork.player.SetCustomProperties(hashtable);
        PhotonNetwork.networkingPeer.OpRaiseEvent(0xff, PhotonNetwork.player.customProperties, true, null);
        PhotonNetwork.player.SetCustomProperties(PhotonNetwork.player.allProperties);
        Reconnectnum++;

    }
    public static int connectcount = 0;
    public void ChangeLocalPlayer(int i)
    {
        if (connectcount < 30)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add(PhotonPlayerProperty.name, i.ToString());
            hashtable.Add(PhotonPlayerProperty.guildName, LoginFengKAI.player.guildname);
            hashtable.Add(PhotonPlayerProperty.kills, 0);
            hashtable.Add(PhotonPlayerProperty.max_dmg, 0);
            hashtable.Add(PhotonPlayerProperty.total_dmg, 0);
            hashtable.Add(PhotonPlayerProperty.deaths, 0);
            hashtable.Add(PhotonPlayerProperty.dead, true);
            hashtable.Add(PhotonPlayerProperty.isTitan, 0);
            hashtable.Add(PhotonPlayerProperty.RCteam, 0);
            hashtable.Add(PhotonPlayerProperty.currentLevel, string.Empty);
            PhotonNetwork.AllocateViewID();
            PhotonNetwork.player.SetCustomProperties(hashtable);
            connectcount++;
        }
        else
        {
            Reconnect(i);
            connectcount = 0;
            
        }
    }

    public PhotonPlayer Get(int id)
    {
        return Find(id);
    }

    public override int GetHashCode()
    {
        return this.ID;
    }

    public PhotonPlayer GetNext()
    {
        return this.GetNextFor(this.ID);
    }

    public PhotonPlayer GetNextFor(PhotonPlayer currentPlayer)
    {
        if (currentPlayer == null)
        {
            return null;
        }
        return this.GetNextFor(currentPlayer.ID);
    }

    public PhotonPlayer GetNextFor(int currentPlayerId)
    {
        if (((PhotonNetwork.networkingPeer == null) || (PhotonNetwork.networkingPeer.mActors == null)) || (PhotonNetwork.networkingPeer.mActors.Count < 2))
        {
            return null;
        }
        Dictionary<int, PhotonPlayer> mActors = PhotonNetwork.networkingPeer.mActors;
        int num = 0x7fffffff;
        int num2 = currentPlayerId;
        foreach (int num3 in mActors.Keys)
        {
            if (num3 < num2)
            {
                num2 = num3;
            }
            else if ((num3 > currentPlayerId) && (num3 < num))
            {
                num = num3;
            }
        }
        return ((num == 0x7fffffff) ? mActors[num2] : mActors[num]);
    }

    internal void InternalCacheProperties(Hashtable properties)
    {
        if (((properties != null) && (properties.Count != 0)) && !this.customProperties.Equals(properties))
        {
            if (properties.ContainsKey((byte) 0xff))
            {
                this.nameField = (string) properties[(byte) 0xff];
            }
            this.customProperties.MergeStringKeys(properties);
            this.customProperties.StripKeysWithNullValues();
        }
    }

    internal void InternalChangeLocalID(int newID)
    {
        if (!this.isLocal)
        {
            Debug.LogError("ERROR You should never change PhotonPlayer IDs!");
        }
        else
        {
            this.actorID = newID;
        }
    }

    public void SetCustomProperties(Hashtable propertiesToSet)
    {
        if (propertiesToSet != null)
        {
            this.customProperties.MergeStringKeys(propertiesToSet);
            this.customProperties.StripKeysWithNullValues();
            Hashtable actorProperties = propertiesToSet.StripToStringKeys();
            if ((this.actorID > 0) && !PhotonNetwork.offlineMode)
            {
                PhotonNetwork.networkingPeer.OpSetCustomPropertiesOfActor(this.actorID, actorProperties, true, 0);
            }
            object[] parameters = new object[] { this, propertiesToSet };
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, parameters);
        }
    }

    public override string ToString()
    {
        if (string.IsNullOrEmpty(this.name))
        {
            return string.Format("#{0:00}{1}", this.ID, !this.isMasterClient ? string.Empty : "(master)");
        }
        return string.Format("'{0}'{1}", this.name, !this.isMasterClient ? string.Empty : "(master)");
    }

    public string ToStringFull()
    {
        return string.Format("#{0:00} '{1}' {2}", this.ID, this.name, this.customProperties.ToStringFull());
    }

    public Hashtable allProperties
    {
        get
        {
            Hashtable target = new Hashtable();
            target.Merge(this.customProperties);
            target[(byte) 0xff] = this.name;
            return target;
        }
    }

    public Hashtable customProperties { get; private set; }

    public int ID
    {
        get
        {
            return this.actorID;
        }
    }

    public bool isMasterClient
    {
        get
        {
            return (PhotonNetwork.networkingPeer.mMasterClient == this);
        }
    }

    public string name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            if (!this.isLocal)
            {
                Debug.LogError("Error: Cannot change the name of a remote player!");
            }
            else
            {
                this.nameField = value;
            }
        }
    }
}

