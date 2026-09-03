# Harvest Season
"Grow Green, Grow Wealthy"

A small 2D farming sim. Plant, water, fertilize, harvest, sell — the usual loop, except prices move with the seasons, so when you buy and sell matters almost as much as what you grow.

This started as a bare-bones prototype just to prove the plant-harvest-sell loop was fun before I built anything else on top of it. It grew from there into a tighter, shorter experience: a full run is 14 days, split between Spring and Summer, and it ends in an actual game-over screen with a summary of how you did.

## How it works

Right now there are two crops, tomatoes and turnips, planted on tilemap plots. A tile only starts growing once it's been both watered and fertilized, and there's a visible growth stage change halfway through so you can tell it's actually progressing.

The economy is the part I spent the most time on. Buy and sell prices are looked up per season, and the season-change screen tells you outright what shifted — seeds get cheaper in Spring, water gets pricier in Summer. It's a small system but it's the whole reason the game exists.

Inventory is a simple list under the hood, with a clean split between tools that never run out (your starting hoe) and consumables that do.

## Built with
Unity, C#, 2D Tilemap

## Where it's at
Feature complete for what it set out to be — a short, focused loop rather than a big open farm sim. Could use a wider crop list and probably a third season if I keep working on it.

## How I worked
I designed the systems — growth timing, the seasonal pricing rules, how everything should hook together — and implemented them in Unity with a good amount of AI-assisted coding, then did the integration and tuning by hand.

## Screenshots
*(adding a shot of a growing tile and the season-transition screen)*
