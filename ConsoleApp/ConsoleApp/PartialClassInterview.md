1.	What constraints apply to partial classes (e.g., same namespace, accessibility)?
All parts must be in the same namespace and assembly, and must use the partial keyword. Accessibility must be consistent across parts (e.g., all public or all internal).
2.	How do partial classes work with code generation?
A tool generates one part (e.g., Person.Generated.cs) while you maintain another part (e.g., Person.Custom.cs). The compiler merges them, so you can add behavior without editing generated code.
3.	Can you have different base classes or interfaces across parts?
You can only have one base class total, and it must be the same across parts. Interfaces can be split across parts; all declared interfaces are combined.
4.	What happens if two parts define the same member?
It’s a compile-time error (duplicate member). The only exception is partial methods, which can have a declaration and an implementation.
5.	How do partial methods work and when are they removed by the compiler?
A partial method can be declared in one part and implemented in another. If no implementation exists, the call is removed at compile time, so there’s no runtime cost.
6.	Are partial classes allowed with struct, interface, or record?
Yes. partial is allowed with class, struct, interface, and record.
7.	Can a partial class be sealed or abstract, and where must those modifiers appear?
Yes. sealed or abstract can appear on any part, but the final combined type must be consistent (you can’t have both).
