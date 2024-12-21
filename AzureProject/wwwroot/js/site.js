var plannedTranslationTask = false;

document.addEventListener('DOMContentLoaded', () => {
    if (window.location.href.includes('Translator')) {
        document.addEventListener('selectionchange', () => {
            let selection = document.getSelection().toString();
            if (selection.length > 1) {
                if (plannedTranslationTask) {
                    clearTimeout(plannedTranslationTask);
                }
                plannedTranslationTask = setTimeout(
                    () => translate(selection), 1000);
            }
        });
    }
});

function translate(text) {
    let langFrom = document.getElementById("lang-from").value;
    let langTo = document.getElementById("lang-to").value;
    fetch(`/Home/Translate?text=${text}&lang-from=${langFrom}&lang-to=${langTo}`)
        .then(r => r.json())
        .then(j => alert(j));
    plannedTranslationTask = false;
}