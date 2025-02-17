# PaperInsight developed by Moritz Langner, Peyman Toreini, Pascal Roller and Alexander Maedche

For **licensing purposes**, we cannot include the Tobii libraries directly within this repository. Therefore, you will need to download and add the libraries yourself as per the instructions below.

## Requiered libraries

- Tobii Interaction Library (WPF)
- Tobii Pro library
- WebView2

## Setup Tobii Interaction Library

1. Download Tobii Interaction Library from the official Tobii website (see https://tobiitech.github.io/interaction-library-docs/sdk/html/index.html)

2. Open the project in **Visual Studio**.

3. **Right-click** on your project in the **Solution Explorer**

4. Select **"Add" > "Reference"**.

5. In the Reference Manager, click **"Browse"** and navigate to the location of the **Tobii.Interaction.Wpf.dll** you downloaded.

6. Select the DLL and click **"Add"**, then "OK".

## Setup Tobii Pro library

1. Open the project in **Visual Studio**.

2. **Right-click** on your project in the **Solution Explorer**.

3. Select **"Manage NuGet Packages..."** from the context menu.

4. In the **Browse** tab, type **`Tobii.Research.x64`** in the search bar.

5. Once the package appears, click **Install**.

6. Review any license agreements and click **Accept** to complete the installation.
