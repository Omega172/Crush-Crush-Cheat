#pragma once
#include <iostream>
#include <vector>

struct Girl
{
    std::string name;
    int id;
};

inline std::vector<Girl> Girls // Balance::GirlName
{
    {"Cassie", 0},
    {"Mio", 1},
    {"Quill", 2},
    {"Elle", 3},
    {"Nutaku", 4},
    {"Iro", 5},
    {"Bonnnibel", 6},
    {"Ayeka", 7},
    {"Fumi", 8},
    {"Bearverly", 9},
    {"Nina", 10},
    {"Alpha", 11},
    {"Pamu", 12},
    {"Luna", 13},
    {"Eva", 14},
    {"Karma", 15},
    {"Sutra", 16},
    //{"Dark One", 17}, // Commented out because these two will auto unlock with the others, and can break shit it force unlocked.
    //{"QPiddy", 18},
    {"Darya", 19},
    {"Jelle", 20},
    {"Quillizone", 21},
    {"Bonchovy", 22},
    {"Spectrum", 23},
    {"Charlotte", 24},
    {"Odango", 25},
    {"Shibuki", 26},
    {"Sirina", 27},
    {"Catara", 28},
    {"Vellatrix", 29},
    {"Peanut", 30},
    {"Roxxy", 31},
    {"Tessa", 32},
    {"Claudia", 33},
    {"Rosa", 34},
    {"Juliet", 35},
    {"Wendy", 36},
    {"Ruri", 37},
    {"Generica", 38},
    {"Suzu", 39},
    {"Lustat", 40},
    {"Sawyer", 41},
    {"Explora", 42},
    {"Esper", 43},
    {"Renee", 44},
    {"Mallory", 45},
    {"Lake", 46},
    {"Brie", 47},
    {"Ranma", 48},
    {"Lotus", 49},
    {"Cassia", 50},
    {"Yuki", 51},
    {"Nova", 52},
    {"Marybelle", 53},
    {"Babybelle", 54},
    {"Pepper", 55},
    {"Amelia", 56},
    {"Kira",57},
    {"Miss Desiree", 58},
    {"Nightingale", 59},
    {"Grace", 60},
    {"Desdemona", 61},
    {"Abby", 62},
    {"Shelly", 63},
    {"Honey", 64},
    {"Karyn", 65},
    {"Myro", 66},
    {"Aurora", 67}
};

enum LoveLevel
{
    Adversary,
    Nuisance,
    Frenemy,
    Acquaintance,
    Friendzoned,
    Awkward_Besties,
    Crush,
    Sweetheart,
    Girlfriend,
    Lover
};

enum DateType
{
    MoonlightStroll = 1,
    CoffeeShop = 2,
    Sightseeing = 4,
    MovieTheater = 8,
    Beach = 16
};

enum BlayFapItems
{
    NutakuUserOutreach = 0,
    Easter2017,
    Summer2017,
    StarterPack,
    July2017,
    BackToSchool2017,
    Ayano2017,
    Darya,
    JelleQuillzone,
    BonchovySpectrum,
    NSFW,
    Winter2018,
    Charlotte,
    Nutaku2019,
    Odango,
    Shibuki,
    Sirina,
    Vellatrix,
    Roxxy,
    Tessa,
    Catara,
    Claudia,
    Juliet,
    Rosa,
    Wendy,
    Ruri,
    Generica,
    Suzu,
    FullVoices,
    Lustat,
    Winter2020,
    Anniversary2021,
    Sawyer,
    Explora,
    Esper,
    MioPlush,
    QuillPlush,
    Renee,
    Mallory,
    Anniversary2022,
    Lake,
    Brie,
    Ranma,
    Lotus,
    Cassia,
    PESStarterPack,
    Yuki,
    RosaAnimated,
    Nova,
    Marybelle,
    Babybelle,
    Pepper,
    Amelia,
    PeanutDate,
    Kira,
    MissDesiree,
    Nightingale,
    Grace,
    Desdemona,
    Abby,
    Shelly,
    Honey,
    Karyn,
    Myro,
    Aurora,
    MAX = 67
};