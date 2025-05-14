# Zobrazovanie viacrozmerných funkcií na GPU

Študent: **Bc. Adrián Kocifaj**  
Školiteľ: **Mgr. Andrej Mihálik, PhD**

Diplomová práca, ktorej cieľom je navrhnúť a implementovať systém na vizualizáciu viacrozmerných funkcií. Pre zobrazenie funkcií s viacerými dimenziami je nutné použiť iné vlastnosti, ako sú farba, priehľadnosť a ďalšie. Zložitosť výpočtu vyžaduje paralelnosť a rýchlosť grafických kariet, to umožní aj zobrazenie komplexných objektov v interaktívnom prostredí.

Zdrojový kód aplikácie: [Zdrojový kód](code/)

Diplomová práca (aktuálna verzia): [Diplomová práca](docs/master_thesis.pdf)

Prezentácia (Projektový seminár 1): [Prezentácia](docs/presentation.pdf)

### **17.02.2025 - 02.03.2025**
- [x] **Stretnutie so školiteľom a prekonzultovanie priebehu práce.**

    Stretnutie prebehlo.

- [x] **Prekonzultovanie problematiky a štruktúry práce.**

    Preberanie problematiky a spôsobov, akým môžu byť zobrazené vyššie dimenzie.

- [x] **Vyhradenie hlavných cieľov na splnenie zadania práce.**

    Vyhradené nasledovné ciele:
    - Implementácia interaktívneho používateľského prostredia.
        - Pohyb v priestore.
        - Zobrazenie určitých častí funkcie.
        - Grafické používateľské rozhranie, nastavenia funkcií a zobrazovania.
        - Možnosť zadávať predpisy funkcií priamo v grafickom rozhraní.
    - Implementácia zobraziteľnosti pre rôzne počty dimenzií.
        - Dimenzie 4, 5, 6.
        - Implementácia viacerých techník zobrazovania a ich porovnanie.
            - Volumetrické zobrazovanie.
            - Ďalšie techniky, vhodný výber pri práci.
    - Použitie viacerých možností pre zobrazenie vyšších dimenzií.
        - Farba.
        - Priehľadnosť.
        - Návrh ďalších použiteľných vlastností.

### **03.03.2025 - 16.03.2025**
- [x] **Analýza dostupných riešení.**

    Preskúmanie viacerých možností (stručne):
    - **OpenGL:** Široká podpora, ale existujú rýchlejšie riešenia.
    - **Vulkan:** Veľmi nízkoúrovňový, preto ťažká implementácia.
    - **WebGL:** Zamerané na zobrazenie grafiky priamo v prehliadači, obmedzený výkon oproti novším technológiám, nepodporuje technológiu compute shader.
    - **WebGPU**: Nová technológia, ktorá nahradzuje WebGL, renderovanie grafiky priamo v prehliadači, neobmedzuje výkon, podporuje technológiu compute shader.
    - **wgpu**: Priama implementácia WebGPU v jazyku Rust, umožňuje vytváranie **WebAssembly** (WASM) aplikácií.

- [x] **Výber konečného riešenia pre implementáciu.**

    Rozhodli sme sa pre použitie **wgpu** s kompiláciou do **WebAssembly** (WASM). Kompilácia zabezpečená pomocou frameworku **eframe**. Hlavným výsledkom bude interaktívna webová aplikácia, pre používateľské rozhranie bola vybraná grafická knižnica **egui**.

### **17.03.2025 - 30.03.2025**
- [x] **Implementácia základnej verzie aplikácie.**

### **31.03.2025 - 13.04.2025**s
- [x] **Prehľad a výber vhodných vedeckých článkov.**

    Prieskum viacerých článkov na tému práce.

    S odporučením školiteľa boli vybrané nasledujúce:
    - [Interactive Volume Illumination of Slice-Based Ray Casting](https://arxiv.org/abs/2008.06134)
        - Efektívny výpočtet osvetlenia v objemových dátach pomocou kombinácie techník **Slice-Based Rendering** a **Ray Casting**.
    
    - [Weighted Blended Order-Independent Transparency](https://jcgt.org/published/0002/02/09/)
        - Jednoduchý a výkonný algoritmus na miešanie priehľadných vrstiev bez potreby ich zoradenia.

### **14.04.2025 - 27.04.2025**
- [x] **Štúdium vybraných vedeckých článkov.**

    Naštudované vybrané vedecké články:
    - [Interactive Volume Illumination of Slice-Based Ray Casting](https://arxiv.org/abs/2008.06134)
    - [Weighted Blended Order-Independent Transparency](https://jcgt.org/published/0002/02/09/)

- [x] **Príprava šablóny diplomovej práce v LaTeX.**
    Nastavenie a pripravenie hlavnej kostry diplomovej práce s preddefinovanými kapitolami. Pripravené na vkladanie naštudovanej dokumentácie.

#### **28.04.2025 – 11.05.2025**
- [x] **Zverejnenie postupu a ďalších informácií na GitHub.**

    Zverejnenie aktuálneho postupu práce, [kódu](code/) a textu [diplomovej práce](docs/master_thesis.pdf) na **GitHub**. Usporiadanie a nastavenie repozitára.

- [x] **Vypracovanie a príprava na prezentáciu.**

    Vytvorenie [prezentácie](docs/presentation.pdf) na predmet **Projektový seminár 1** a príprava na jej odprezentovanie.

- [x] **Stretnutie so školiteľom.**

    Stretnutie ohľadom doterajších výsledkov a budúceho postupu práce.
