using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace PeteTimesSix.CategorizedCleaning
{
    [StaticConstructorOnStartup]
    public static class Textures_Custom
    {
        public static Texture2D ArrowLeft { get; internal set; } = ContentFinder<Texture2D>.Get("CategorizedCleaning/ArrowLeft", true);
        public static Texture2D ArrowRight { get; internal set; } = ContentFinder<Texture2D>.Get("CategorizedCleaning/ArrowRight", true);
    }
}
