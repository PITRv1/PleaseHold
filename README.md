# Please Hold
<p align="center">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Assets/_Assets/Images/GameLogoForWeb.png?raw=true" style="height:200px">
</p>
<p align="center">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Assets/_Assets/Images/undefined_team_logo.png?raw=true" style="width:200px">
</p>

## A játék történetéről
Munkánk során gyakran kell felhívnunk a Kormányablakot, akik sokszor megváratnak minket. Egyszer megunjuk és elkezdünk játszani a telefonunkon.

## Fontos programok és verzióik
- Microsoft Office 2019
- Unity Hub 3.11.1
- Unity 6 (6000.0.40f1)
- Dotnet 8

## A feladatról
Ezt a játékot a Vadász Dénes Informatika Versenyre készítettük. A feladatról röviden:

### 1. Adatbázis
<img src="https://imgs.search.brave.com/7dCTcjvsnCBTX-JiFSrTbGxqDNvF2OBUNpHc8irTBoc/rs:fit:500:0:0:0/g:ce/aHR0cHM6Ly91cGxv/YWQud2lraW1lZGlh/Lm9yZy93aWtpcGVk/aWEvY29tbW9ucy90/aHVtYi9mL2YxL01p/Y3Jvc29mdF9PZmZp/Y2VfQWNjZXNzXyUy/ODIwMTktcHJlc2Vu/dCUyOS5zdmcvMjIw/cHgtTWljcm9zb2Z0/X09mZmljZV9BY2Nl/c3NfJTI4MjAxOS1w/cmVzZW50JTI5LnN2/Zy5wbmc" style="width: 50px">

- Létre kellett hozni egy adatbázist MS Access-ben különböző táblákkal  és integritási szabályokkal. A Rekordokat lekérdezésekben kellet megjeleníteni.
- Minimum adatmennyiségek: 10 épület, 30 lakos, 10 szolgáltatás és 5 városfejlesztési projekt.

### 2. Statisztika

<img src="https://imgs.search.brave.com/tDP1FFHMa_p_IhPueRD1KbzpoQo5f-BQBbZMqJSjbrk/rs:fit:500:0:0:0/g:ce/aHR0cHM6Ly91cGxv/YWQud2lraW1lZGlh/Lm9yZy93aWtpcGVk/aWEvY29tbW9ucy90/aHVtYi8zLzM0L01p/Y3Jvc29mdF9PZmZp/Y2VfRXhjZWxfJTI4/MjAxOSVFMiU4MCU5/M3ByZXNlbnQlMjku/c3ZnLzIyMHB4LU1p/Y3Jvc29mdF9PZmZp/Y2VfRXhjZWxfJTI4/MjAxOSVFMiU4MCU5/M3ByZXNlbnQlMjku/c3ZnLnBuZw" style="width: 50px">

A táblákat be kellett importálni MS Excel-be, ahol statisztikát kellett készíteni a következő pontokról:
 - Az épülettípusok aránya a városban.
 - Lakosok korcsoportok szerinti megoszlása.
 - A fejlesztési költségek összesítése épület-típusonként csoportosítva.

### 3. Városfejlesztési szimulátor

<img src="https://imgs.search.brave.com/skcf_DAStfWCngkl1QhR7AYwSEwbmpEAOlU1MnEPba0/rs:fit:200:200:1:0/g:ce/aHR0cHM6Ly9wcmV2/aWV3LnJlZGQuaXQv/dHUzZ3Q2eXNmeHE3/MS5wbmc_YXV0bz13/ZWJwJnM9MTBhYjU1/ZDlkYzA5ZTdlZDZl/YTU5YmQ1OTE2ODAw/YTUyNzJkNTk2OQ" style="width: 60px;">

A program megtervezése előtt megállapodtunk abban, hogy ebből a feladatból egy játékot szeretnénk készíteni. A kivitelezéshez a C# programozási nyelvet és a Unity játékmotort választottuk.

## Használat
A bevezető videó után a játék átvált a főmenüre, ahol:
<p align="center">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Tutorial%20images/1.png?raw=true" style="width:1000px">
</p>

 - a **[Continue]** gommbbal folytathatja a már megkezdett, de nem befejezett szimulációt
 - a **[Play]** gombbal indíthat egy új szimulációt
 - az **[Options]** gombbal megnyithatja a beállításokat
 - a **[Credits]** gombbal olvashat a fejlesztői csapatról
 - az **[Exit]** gombbal bezárhatja a programot

A **[Play]** lenyomása után megjelenik jobb oldalon egy olyan menü, ahol megadhatja a szimuláció paramétereit:
<p align="center">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Tutorial%20images/2.png?raw=true" style="width:1000px">
</p>

> [!NOTE]  
> A **[Load Default Test Files]** gomb lenyomására a program kiolvassa a csv állományokat a **(Játék mappája)/PleaseHold/Assets/CSV Files/** mappából. Ebbe a mappába a saját fájljait is elhelyezheti.

Itt tudja megadni a:
 - szimuláció kezdő évét és hónapját
 - a szimuláció hosszát hónapokban
 - a lakosok boldogságának minimum és kezdeti értékét
 - a kezdő költségvetést
 - az épületek állapotát
 - a szolgáltatások fentartási költségét

<p align="center">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Tutorial%20images/3.png?raw=true" style="width:1000px">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Tutorial%20images/4.png?raw=true" style="width:1000px">
</p>
A szolgáltatásnál meg tudja adni manuálisan vagy a fenntartás minimum és maximum értékeit megadva randomizálni is tudja ezeket. 
<p align="center">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Tutorial%20images/5.png?raw=true" style="width:1000px">
</p>

A **[Start simulation]** gomb lenyomása után elindíthatja a szimulációt.
<p align="center">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Tutorial%20images/6.png?raw=true" style="width:1000px">
</p>
Bal felül láthatja az adott dátumot, középen a lakosok boldogságát és jobb oldalt a rendelkezésre álló pénzösszeget.
Kamera irányítása:
 - jobb shift + egér görgő nyomvatartása + egér mozgatása: a kamera mozgatása
 - egér görgő nyomvatartása + egér mozgatésa: a kamera forgatása
 - görgő: ráközelítés

<p align="center">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Tutorial%20images/7.png?raw=true" style="width:500px">
</p>

Az épületek adatait úgy tudja lekérni, ha ráviszi a kurzort.
Ha jobb klikkel egy üres területre, akkor megnyiílik az épület építése menü. Itt meg kell adnia a(z):
 - épület nevét
 - típusát
 - méretét
 - megépítéshez szükséges hónapok száma

<p align="center">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Tutorial%20images/8.png?raw=true" style="width:500px">
</p>

Ha jobb klikkel egy épületre, akkor megjavíthatja azt. Meg kell adnia a:
 - a felújításra szánt költségvetés
 - a felújításhoz szükséges hónapok száma

<p align="center">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Tutorial%20images/9.png?raw=true" style="width:500px">
</p>

A burger menüre kattintva megjelenik 3 opció:
 - új szolgáltatás létrehozása
 - szolgáltatás törlése
 - új projekt létrehozása

<p align="center">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Tutorial%20images/10.png?raw=true" style="width:1000px">
</p>

Új szolgáltatás létrehozásához a következő adataok szükségesek:
 - szolgáltatás neve
 - szolgáltatás típusa
 - kapcsolódó épületek
 - szolgáltatásra szánt költségvetés

<p align="center">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Tutorial%20images/10.png?raw=true" style="width:1000px">
</p>

Szolgáltatás leállításhoz ki kell választania, hogy melyiket szeretné leáálítani.

<p align="center">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Tutorial%20images/11.png?raw=true" style="width:1000px">
</p>

Új projekt létrehozásához meg kell adnia a:
 - projekt nevét
 - projekt típusát
 - projekt megvalósítására szánt hónapok számát
 - kapcsolódó épületeket
 - projekt költségvetését

A játék közben váratlan események is történhetnek, amiket alul középen fog látni.

<p align="center">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Tutorial%20images/13.png?raw=true" style="width:1000px">
</p>

A következő képen láthatja azt a menüt, amikor véget ér a szimuláció.

<p align="center">
  <img src="https://github.com/PITRv1/PleaseHold/blob/master/Tutorial%20images/14.png?raw=true" style="width:1000px">
</p>
