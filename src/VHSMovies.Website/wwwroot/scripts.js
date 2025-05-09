window.scrollToSection = (element, headerOffset) => {
    if (!element || !element.getBoundingClientRect) return;

    const rect = element.getBoundingClientRect();
    const offset = rect.top + window.scrollY - headerOffset;

    window.scrollTo({
        top: offset,
        behavior: "smooth"
    });
};