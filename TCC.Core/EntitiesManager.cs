﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TCC.Data;
using TCC.Data.Databases;
using TCC.ViewModels;

namespace TCC
{
    public static class EntitiesManager
    {
        public static ObservableCollection<Player> CurrentUsers = new ObservableCollection<Player>();
        public static MonsterDatabase CurrentDatabase;
        private static ulong currentEncounter;
        public static Dragon CurrentDragon = Dragon.None;
        public static ObservableCollection<ulong> chestList = new ObservableCollection<ulong>();
        public static void SpawnNPC(ushort zoneId, uint templateId, ulong entityId, Visibility v, bool force)
        {
            if (CurrentDatabase.TryGetMonster(templateId, zoneId, out Monster m))
            {
                if (m.IsBoss || force)
                {
                    BossGageWindowManager.Instance.AddOrUpdateBoss(entityId, m.MaxHP, m.MaxHP, templateId, zoneId, v);
                }
            }
            //Console.WriteLine("+Bosses {0} - {1}",BossGageWindowManager.Instance.VisibleBossesCount, BossGageWindowManager.Instance.CurrentNPCs.Count);
        }
        public static void DespawnNPC(ulong target)
        {
            BossGageWindowManager.Instance.RemoveBoss(target);
            //Console.WriteLine("-Bosses {0} - {1}", BossGageWindowManager.Instance.VisibleBossesCount, BossGageWindowManager.Instance.CurrentNPCs.Count);
            if (BossGageWindowManager.Instance.VisibleBossesCount == 0) SessionManager.Encounter = false;
        }
        public static void SetNPCStatus(ulong entityId, bool enraged)
        {
            BossGageWindowManager.Instance.SetBossEnrage(entityId, enraged);
        }
        public static void UpdateNPCbyGauge(ulong target, float curHP, float maxHP, ushort zoneId, uint templateId)
        {
            BossGageWindowManager.Instance.AddOrUpdateBoss(target, maxHP, curHP, templateId, zoneId, Visibility.Visible);
            //Console.WriteLine("{0}/{1}", curHP, maxHP);
            //Console.WriteLine("++Bosses {0} - {1}", BossGageWindowManager.Instance.VisibleBossesCount, BossGageWindowManager.Instance.CurrentNPCs.Count);

            if (maxHP > curHP)
            {
                currentEncounter = target;
                SessionManager.Encounter = true;
            }
            else if(maxHP == curHP || curHP == 0)
            {
                currentEncounter = 0;
                SessionManager.Encounter = false;
            }
        }
        public static void ClearNPC()
        {
            BossGageWindowManager.Instance.ClearBosses();
        }
        public static void ClearUsers()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                CurrentUsers.Clear();
            });
        }
        public static void CheckHarrowholdMode(ushort zoneId, uint templateId)
        {
            if (zoneId == 1023) return;
            if (zoneId == 63 && templateId >= 1960 && templateId <= 1963) return;
            if (zoneId != 950)
            {
                SessionManager.HarrowholdMode = false;
                //System.Console.WriteLine("{0} {1} spawned, exiting hh mode", zoneId, templateId);
            }
            else
            {
                if (templateId >= 1100 && templateId <= 1103)
                {
                    SessionManager.HarrowholdMode = true;
                    //System.Console.WriteLine("{0} {1} spawned, entering hh mode", zoneId, templateId);

                    SetDragonsContexts(templateId);
                }
                else if (templateId == 1000 || templateId == 2000 || templateId == 3000 || templateId == 4000)
                {
                    SessionManager.HarrowholdMode = false;
                    //System.Console.WriteLine("{0} {1} spawned, exiting hh mode", zoneId, templateId);
                }
            }

        }
        public static void CheckCurrentDragon(Point p)
        {
            var rel = Utils.GetRelativePoint(p.X, p.Y, -7672, -84453);

            Dragon d;
            if (rel.Y > .8 * rel.X - 78)
            {
                if (rel.Y > -1.3 * rel.X - 94)
                {
                    d = Dragon.Aquadrax;

                }
                else
                {
                    d = Dragon.Umbradrax;
                }
            }
            else
            {
                if (rel.Y > -1.3 * rel.X - 94)
                {
                    d = Dragon.Terradrax;
                }
                else
                {
                    d = Dragon.Ignidrax;
                }
            }
            if (EntitiesManager.CurrentDragon != d)
            {
                EntitiesManager.CurrentDragon = d;
                WindowManager.BossGauge.HHBosses.Select(EntitiesManager.CurrentDragon);
            }

        }
        static void SetDragonsContexts(uint templateId)
        {
            //if (templateId == 1100)
            //{
            //    WindowManager.BossGauge.Dispatcher.Invoke(() =>
            //    {
            //        WindowManager.BossGauge.HHBosses.ignidrax.Reset();
            //        WindowManager.BossGauge.HHBosses.ignidrax.DataContext = CurrentBosses.FirstOrDefault(x => x.Name == "Ignidrax");
            //    });
            //}
            //if (templateId == 1101)
            //{
            //    WindowManager.BossGauge.Dispatcher.Invoke(() =>
            //    {
            //        WindowManager.BossGauge.HHBosses.terradrax.Reset();
            //        WindowManager.BossGauge.HHBosses.terradrax.DataContext = CurrentBosses.FirstOrDefault(x => x.Name == "Terradrax");
            //    });
            //}
            //if (templateId == 1102)
            //{
            //    WindowManager.BossGauge.Dispatcher.Invoke(() =>
            //    {
            //        WindowManager.BossGauge.HHBosses.umbradrax.Reset();
            //        WindowManager.BossGauge.HHBosses.umbradrax.DataContext = CurrentBosses.FirstOrDefault(x => x.Name == "Umbradrax");
            //    });
            //}
            //if (templateId == 1103)
            //{
            //    WindowManager.BossGauge.Dispatcher.Invoke(() =>
            //    {
            //        WindowManager.BossGauge.HHBosses.aquadrax.Reset();
            //        WindowManager.BossGauge.HHBosses.aquadrax.DataContext = CurrentBosses.FirstOrDefault(x => x.Name == "Aquadrax");
            //    });
            //}

        }

        public static void SpawnUser(ulong entityId, string name)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                CurrentUsers.Add(new Player(entityId, name));
            });
        }
        public static void DespawnUser(ulong entityId)
        {
            if (TryGetUserById(entityId, out Player p))
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    CurrentUsers.Remove(p);
                });
            }
        }

        static void UnsetDragonsContexts(ulong target)
        {
            WindowManager.BossGauge.Dispatcher.Invoke(() =>
            {
                if (WindowManager.BossGauge.HHBosses.ignidrax.EntityId == target)
                {
                    WindowManager.BossGauge.HHBosses.ignidrax.DataContext = null;
                    WindowManager.BossGauge.HHBosses.ignidrax.ForceEnrageOff();
                    WindowManager.BossGauge.HHBosses.abnormalities.DataContext = null;
                    WindowManager.BossGauge.HHBosses.abnormalities.ItemsSource = null;
                }
                if (WindowManager.BossGauge.HHBosses.aquadrax.EntityId == target)
                {
                    WindowManager.BossGauge.HHBosses.aquadrax.DataContext = null;
                    WindowManager.BossGauge.HHBosses.aquadrax.ForceEnrageOff();
                    WindowManager.BossGauge.HHBosses.abnormalities.DataContext = null;
                    WindowManager.BossGauge.HHBosses.abnormalities.ItemsSource = null;

                }
                if (WindowManager.BossGauge.HHBosses.terradrax.EntityId == target)
                {
                    WindowManager.BossGauge.HHBosses.terradrax.DataContext = null;
                    WindowManager.BossGauge.HHBosses.terradrax.ForceEnrageOff();
                    WindowManager.BossGauge.HHBosses.abnormalities.DataContext = null;
                    WindowManager.BossGauge.HHBosses.abnormalities.ItemsSource = null;

                }
                if (WindowManager.BossGauge.HHBosses.umbradrax.EntityId == target)
                {
                    WindowManager.BossGauge.HHBosses.umbradrax.DataContext = null;
                    WindowManager.BossGauge.HHBosses.umbradrax.ForceEnrageOff();
                    WindowManager.BossGauge.HHBosses.abnormalities.DataContext = null;
                    WindowManager.BossGauge.HHBosses.abnormalities.ItemsSource = null;


                }
            });
        }

        public static bool TryGetUserById(ulong id, out Player p)
        {
            p = CurrentUsers.FirstOrDefault(x => x.EntityId == id);
            if (p == null)
            {
                p = new Player();
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
