# CSPsharp (CSP#)
## .NET Core implementation of Communicating Sequencial Processes (CSP)

CSP# is an implementation of Communicating Sequential Processes (CSP) for .NET environments. It is based on an existing,
well established Java implementation, JCSP. There is paper, currently in development, that describes the process of implementation with the details of the obstacles
overcome and the results. The project has delivered a fully functional and robust CSP implementation written in C#. It is the only
valid option for .NET developers at present. CSP# is an object oriented CSP library aiding process-oriented programming for the
.NET Core Framework. Similar to JCSP, it relies on the system-based threads.

This implementation contains CSP concepts such as `Process`, `Channel`, `Timer`, `Guard`, `Alternative`, `Barrier`, `Bucket` and `AltingBarrier`. The networking package is based on an updated version of original JCSP networking package. Unfortunately, unlike JCSP, CSP# lacks any GUI packages and cannot be used with any user Interface directly because this capability not being present in .NET Core.

The library was evaluated with multiple examples based on previous work, the JCSP documentation and with additional custom
tests created for the purpose of this project. CSP# is the biggest, most feature-rich and only fully operational CSP based library for
.NET. Apart from JCSP this is the only library containing `Bucket` and `AltingBarrier` concepts.

The tests and examples mentioned in the previous paragraph can be found on the author's GitHub. These projects are called [*CSPsharp-UCaPE*](https://github.com/PasierbKarol/CSPsharp-UCaPE_Examples) and [*CSPsharp-docs*](https://github.com/PasierbKarol/CSPsharp_docs-Examples).

# Installation
The library can be added directly to your project. If you do not wish to see the code, you can open the solution and publish all the parts of the library. Then import these DLLs to your project.
The library is also available on NuGet. You need to obtain all parts separately. [`cspsharp-lang`](https://www.nuget.org/packages/cspsharp-lang/), [`cspsharp-net2`](https://www.nuget.org/packages/cspsharp-net2/), [`cspsharp-plug-n-play`](https://www.nuget.org/packages/cspsharp-plug-n-play/). `cspsharp-utils` is included in `cspsharp-lang`. You can use the Package Manager.

# How to use?
Not certain where to start and how does that work? The best place to start would be to read through the free books [*Using Concurrency and Parallelism Effectively*](https://bookboon.com/en/using-concurrency-and-parallelism-effectively-i-ebook). These books and the JCSP used in the examples were the base for the library conversion achieved in this project. The discussed examples, language and the operation can be identically applied to the CSP#. 

It could be a good idea to look at the [*CSPforJava*](https://github.com/CSPforJAVA/jcsp) on GitHub and check the Documentation examples provided there. Similarities between Java and C# should ease the understanding of the content. Also, some of these examples were also translated to CSP# and can be found on GitHub.

# Want to know more?
There is a paper (currently in development) describing the conversion proces, that also explain some mechanics. It's also worthwile to read original paper by Tony Hoare and papers about other attempts to create CSP.NET from [Kevin Chalmers](https://www.researchgate.net/publication/221004333_CSP_for_NET_Based_on_JCSP) and [Alex Lehmberg](https://www.semanticscholar.org/paper/An-Introduction-to-CSP.NET-Lehmberg-Olsen/d58a0bf030ec79a8ff93dbd63dd22136f8559354).