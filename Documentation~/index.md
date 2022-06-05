<!-- Header -->
<!--<h3 align="center">Project Title</h3>-->
<!--<h2 align="center">Project Description</h2>-->

<p align="center">
	<img src ="https://github-readme-stats-jameslafritz.vercel.app/api/pin?username=JamesLaFritz&repo=GraphViewBehaviorTree&theme=react" alt="Git Repo Title and Info" title="Repo Info"/>'
	<br />
	<a href="https://github.com/JamesLaFritz/GraphViewBehaviorTree/issues">Report Bug</a>
        ·
        <a href="https://github.com/JamesLaFritz/GraphViewBehaviorTree/issues">Request Feature</a>
</p>

<!-- PROJECT SHIELDS -->
<p align="center">
  <a href="https://github.com/JamesLafritz/GraphViewBehaviorTree/graphs/contributors">
	  <img src="https://img.shields.io/github/contributors/JamesLafritz/GraphViewBehaviorTree.svg?style=for-the-badge" title="forks Badge" alt="forks Badge"/>
  </a>
  <a href="https://img.shields.io/github/forks/JamesLafritz/GraphViewBehaviorTree.svg?style=for-the-badge">
	  <img src="https://img.shields.io/github/forks/JamesLafritz/GraphViewBehaviorTree.svg?style=for-the-badge" title="stargazers Badge" alt="stargazers Badge"/>
  </a>
  <a href="https://github.com/JamesLafritz/GraphViewBehaviorTree/stargazers">
	  <img src="https://img.shields.io/github/stars/JamesLafritz/GraphViewBehaviorTree.svg?style=for-the-badge" title="Contributors Badge" alt="Contributors Badge"/>
  </a>
  <a href="https://github.com/JamesLafritz/GraphViewBehaviorTree/issues">
	  <img src="https://img.shields.io/github/issues/JamesLafritz/GraphViewBehaviorTree.svg?style=for-the-badge" title="issues Badge" alt="issues Badge"/>
  </a>
  <a href="https://img.shields.io/github/license/JamesLafritz/GraphViewBehaviorTree.svg?style=for-the-badge">
	  <img src="https://img.shields.io/github/license/JamesLafritz/GraphViewBehaviorTree.svg?style=for-the-badge" title="License Badge" alt="License Badge"/>
  </a>
</p>

<!-- Links -->
<p align="center">
  <a href="https://jameslafritz.intensive.gamedevhq.com/">
	  <img src="https://img.shields.io/badge/Portfolio-21759B?style=for-the-badge&logo=wordpress&logoColor=white" title="Portfolio Badge" alt="Portfolio"/>
  </a>
  <a href="https://ktmarine1999.medium.com/">
	  <img src="https://img.shields.io/badge/Articles-000000?style=for-the-badge&logo=medium&logoColor=white" title="medium Badge" alt="medium"/>
  </a>
  <a href="https://www.linkedin.com/in/james-lafritz/">
	  <img src="https://img.shields.io/badge/LinkedIn-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white" title="LinkedIn Badge" alt="LinkedIn"/>
  </a> 
  <a href="https://ktmarine1999.itch.io/">
	  <img src="https://img.shields.io/badge/Itch-fa5c5c.svg?style=for-the-badge&logo=Itch.io&logoColor=white" title="Itch.io Badge" alt="Itch.io"/>
  </a> 
</p>


<!-- PROJECT LOGO -->
<p align="center">
  <a href="https://github.com/JamesLaFritz/GraphViewBehaviorTree">
    <img src="Images/Logo.png" alt="Logo" width="256"/>
  </a>
</p>

<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary><h2 style="display: inline-block">Table of Contents</h2></summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
        <ul>
            <li> Node Types </li>
            <ul>
                <li><a href="#decorator-node">Decorator Node</a></li>
            </ul>
            <ul>
                <li><a href="#composite-node">Composite Node</a></li>
            </ul>
	        <ul>
                <li><a href="#action-node">Action Node</a></li>
            </ul>
        </ul>
	    <ul>
            <li><a href="#built-with">Built With</a></li>
        </ul>
        <ul>
            <li><a href="#articles">Articles</a></li>
        </ul>
    </li>
    <li><a href="#license">License</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

![Product Name Screen Shot](Images/ScreenShot.gif)

I had code on sitting around on my hard drive for a Behavior Tree Editor That inspired me to create a bBehavior Tree Editor and write Articles about Behavior Tree and Using the Unity Editor UI Builder.
It has been [Pointed Out](https://github.com/JamesLaFritz/GraphViewBehaviorTree/issues/1) to me very polite and professional that this code is actually from the KIWI Coder.

[![The Kiwi Coder](https://thekiwicoder.com/wp-content/uploads/2020/02/site_logo.png)](https://thekiwicoder.com/)

Behavior Tree using UI Builder, GraphView, and Scriptable Objects in Unity 2021.3

This can be used as a template to build any type on Node Based Editor in Unity. i.e Dialog System or an AI Behavior Tree.
If using for AI Behavior tree I would probably use Single Input Ports not Multiple.
Even though the Nodes themselves are Scriptable Objects this can be changed and there is nothing that states that you have to use Scriptable Objects as the type for the nodes, this was just a decision that I made as it is easier to Bing the properties off the Node. 
Also there is nothing sating that the Nodes have to be stored in the Asset itself as I have done. 

Behavior Tree is an execution tree (uses the [Strategy pattern](https://blog.devgenius.io/strategy-pattern-in-unity-b82065aaa969)) and always starts with a Root Node. This will be so
that the behavior tree knows where to start. Root Node has only one child. Now there are 3 basic main type of nodes
Decorator, Composite Node and Action Node.
Each Node can return one of three states Running, Success, or Failure.
All nodes will be saved in Unity as Scriptable Objects. A Behavior tree will be a Scriptable Object containing all the
Nodes in it.

#### Decorator Node
* Has one child and is capable of augmenting the return state of it's child. This uses the [Decorator
pattern](https://blog.devgenius.io/the-decorator-pattern-in-unity-6791ab10b64).

#### Composite Node
* Has a list of children and is the control flow of the behavior tree like switch statements and for
loops. There are 2 types Composite Nodes the Selector and Sequence. This uses the [Composite pattern](https://blog.devgenius.io/composite-pattern-in-unity-fc90e60c946f).

#### Action Node
* The Leaf of the tree, has no children, and is where all of the logic gets implemented.


### Built With

* <a href="https://www.linkedin.com/in/james-lafritz/"><img src="https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white"/></a>

<!-- Installation -->
### Installation
To install this into your project for modifications
Clone or Download the code from Git Hub into the Asset Folder of your project.
Change the Folder From GraphView Behavior Tree to mach what you wold like.
Edit all Files and change the NameSpace to match your new Folder Name. Do Not forget to change this in the UXML Documents as well. i.e "GraphViewBehaviorTree.Editor.SplitView"
Delete the Runtime/james.lafritz.GraphViewBehaviorTree and Editor/james.lafritz.GraphViewBehaviorTree.Editor Assembly Definition Files, Or Edit/Replace them with your own.

To use Template as is Use the Package manager to Install package from Git Hub. 
see [Creating custom packages for use in Unity](https://blog.devgenius.io/creating-custom-packages-for-use-in-unity-7dfbaa49e4b4)

* In Unity open the package manager
    * Go to Window->PackageManager
    * ![OpenPackageManager](Images/OpenPackageManager.gif)
* Install this Package from git url
    * Click the Plus Drop down
    * Select "add package from git URL"
        * https://github.com/JamesLaFritz/GraphViewBehaviorTree.git
        * Select add
    * ![InstallPackage](Images/InstallPackage.gif)

<!-- Articles -->
### Articles

Behavior Trees In Unity](https://ktmarine1999.medium.com/behavior-trees-in-unity-20a738b5508c)

[Using Unity’s UI Builder: Basic Set Up](https://blog.devgenius.io/using-unitys-ui-builder-a86faf17bf27)

[Using Unity’s UI Builder: Serialized Object data binding](https://blog.devgenius.io/using-unitys-ui-builder-bc058e1c7d17)

[Using Unity’s UI Builder: Adding Custom Controls](https://blog.devgenius.io/using-unitys-ui-builder-5e793a90a5ae)

[Using Unity’s Graph View: Adding and Deleting Nodes](https://blog.devgenius.io/using-unitys-graph-view-e9fb8e78980e)

[Using Unity’s Graph View: Node Position, Code/Editor Window Reload](https://ktmarine1999.medium.com/using-unitys-graph-view-18b38a23dea5)

[Using Unity’s Graph View: Connecting Nodes in The Editor](https://ktmarine1999.medium.com/4cc55b704548)

[Inspector View in an Editor Window](https://medium.com/@ktmarine1999/inspector-view-in-an-editor-window-5debd86cbe6f)



<!-- ROADMAP -->
## Roadmap

See the [open issues](https://github.com/JamesLaFritz/GraphViewBehaviorTree/issues) for a list of proposed features (and known issues).



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.


<!-- ACKNOWLEDGEMENTS -->
## Acknowledgements

[![The Kiwi Coder Behavior Tree Editor](https://thekiwicoder.com/wp-content/uploads/2021/07/behaviour_tree-2.jpg)](https://thekiwicoder.com/behaviour-tree-editor/)
[![The Kiwi Coder](https://thekiwicoder.com/wp-content/uploads/2020/02/site_logo.png)](https://thekiwicoder.com/)


<!--
Repo Card Exclusive Options:
    show_owner - Show the repo's owner name (boolean)

Common Options:
    title_color - Card's title color (hex color)
    text_color - Body text color (hex color)
    icon_color - Icons color if available (hex color)
    border_color - Card's border color (hex color). (Does not apply when hide_border is enabled)
    bg_color - Card's background color (hex color) or a gradient in the form of angle,start,end
    hide_border - Hides the card's border (boolean)
    theme - name of the theme, choose from all available themes
    cache_seconds - set the cache header manually (min: 1800, max: 86400)
    locale - set the language in the card (e.g. cn, de, es, etc.)
    border_radius - Corner rounding on the card_
Gradient in bg_color

You can provide multiple comma-separated values in bg_color option to render a gradient, the format of the gradient is :-

&bg_color=DEG,COLOR1,COLOR2,COLOR3...COLOR10

Avaliable Repo Card Themes
default_repocard
dark
radical
merko
gruvbox
tokyonight
onedark
cobalt
synthwave
highcontrast
dracula
prussian
monokai
vue
vue-dark
shades-of-purple
nightowl
buefy
blue-green
algolia
great-gatsby
darcula
bear
solarized-dark
solarized-light
chartreuse-dark
nord
gotham
material-palenight
graywhite
vision-friendly-dark
ayu-mirage
midnight-purple
calm
flag-india
omni
react
jolly
maroongold
yeblu
blueberry
slateorange
kacho_ga
outrun
-->
