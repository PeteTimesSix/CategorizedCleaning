using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace PeteTimesSix.CategorizedCleaning
{
    public class CategorizedCleaning_Settings: ModSettings
    {
        public const float LIST_MARGIN = 20;
        public const float LIST_DESIRED_HEIGHT = 400;
        public const float LIST_DESIRED_WIDTH = 260;

        public const float SCROLLBAR_WIDTH = 18f;
        public const float DEF_BUTTON_HEIGHT = 32;
        public const float ARROW_WIDTH = 32;

        public static List<RoomRoleDef> sterileRooms = new() { RoomRoleDefOf.Hospital, RoomRoleDefOf.Laboratory, RoomRoleDefOf_Custom.Kitchen };
        public static List<RoomRoleDef> outdoorRooms = new() { RoomRoleDefOf_Custom.Barn };

        private static RoomRoleDef[] noneRoom = new RoomRoleDef[] { RoomRoleDefOf.None };

        private Vector2 leftListScrollPos = new Vector2();
        private Vector2 middleListScrollPos = new Vector2();
        private Vector2 rightListScrollPos = new Vector2();

        public void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();

            listing.Begin(inRect);

            //listing.Label("CC_Settings_Header".TranslateSimple());
            
            var listsRect = listing.GetRect(LIST_DESIRED_HEIGHT);
            listsRect = listsRect.MiddlePartPixels(Math.Min(inRect.width, LIST_DESIRED_WIDTH * 3), listsRect.height);

            var regularRooms = DefDatabase<RoomRoleDef>.AllDefsListForReading.Except(sterileRooms).Except(outdoorRooms).Except(noneRoom).ToList();

            DrawDefsList("CC_Settings_SterileRooms", listsRect.LeftPartPixels(listsRect.width / 3).ContractedBy(1f), ref leftListScrollPos, sterileRooms, 
                moveRight: def => sterileRooms.Remove(def));

            DrawDefsList("CC_Settings_RegularRooms", listsRect.MiddlePartPixels(listsRect.width / 3, listsRect.height).ContractedBy(1f), ref middleListScrollPos, regularRooms, 
                moveLeft: def => sterileRooms.Add(def), 
                moveRight: def => outdoorRooms.Add(def));

            DrawDefsList("CC_Settings_OutdoorRooms", listsRect.RightPartPixels(listsRect.width / 3).ContractedBy(1f), ref rightListScrollPos, outdoorRooms, 
                moveLeft: def => outdoorRooms.Remove(def));

            listing.End();
        }

        public void DrawDefsList<T>(string label, Rect rect, ref Vector2 scrollPos, List<T> defs, Action<T> moveLeft = null, Action<T> moveRight = null) where T : Def
        {
            Widgets.DrawBoxSolidWithOutline(rect, Color.black, Color.grey);

            var anchor = Text.Anchor;
            Text.Anchor = TextAnchor.LowerCenter;

            var labelRect = rect.TopPartPixels(24f);
            Widgets.LabelFit(labelRect, label.TranslateSimple());
            Widgets.DrawLineHorizontal(labelRect.x, labelRect.y + labelRect.height, labelRect.width);

            rect.y += labelRect.height + 2;
            rect.height -= labelRect.height + 2 + 2;
            rect = rect.ContractedBy(1f);

            var viewRect = new Rect(0, 0, rect.width, (DEF_BUTTON_HEIGHT + 1) * defs.Count);

            bool hasScrollbar = rect.height < viewRect.height;
            if (hasScrollbar)
                viewRect.width -= SCROLLBAR_WIDTH; //clear space for scrollbar
            Widgets.BeginScrollView(rect, ref scrollPos, viewRect);

            var buttonRect = viewRect.ContractedBy(2f).TopPartPixels(DEF_BUTTON_HEIGHT);

            foreach (var def in defs.OrderBy(d => d.label))
            {
                DrawDefButton(buttonRect, def, moveLeft, moveRight);
                buttonRect.y += DEF_BUTTON_HEIGHT + 1;
            }

            Text.Anchor = anchor;

            Widgets.EndScrollView();
        }

        public void DrawDefButton<T>(Rect buttonRect, T def, Action<T> moveLeft, Action<T> moveRight) where T : Def
        {
            Widgets.DrawHighlightIfMouseover(buttonRect);
            Widgets.LabelFit(buttonRect, def.LabelCap);
            if (moveLeft != null)
            {
                if (Widgets.ButtonImageWithBG(buttonRect.LeftPartPixels(ARROW_WIDTH), Textures_Custom.ArrowLeft))
                    moveLeft(def);
            }
            if (moveRight != null)
            {
                if (Widgets.ButtonImageWithBG(buttonRect.RightPartPixels(ARROW_WIDTH), Textures_Custom.ArrowRight))
                    moveRight(def);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();

            InitializeCollectionsIfNeeded();

            Scribe_Collections.Look(ref sterileRooms, "sterileRooms", LookMode.Def);
            Scribe_Collections.Look(ref outdoorRooms, "outdoorRooms", LookMode.Def);

            InitializeCollectionsIfNeeded();
        }

        public void InitializeCollectionsIfNeeded() 
        {
            if (sterileRooms == null)
                sterileRooms = new() { RoomRoleDefOf.Hospital, RoomRoleDefOf.Laboratory, RoomRoleDefOf_Custom.Kitchen };
            if (outdoorRooms == null)
                outdoorRooms = new() { RoomRoleDefOf_Custom.Barn };
        }
    }
}
