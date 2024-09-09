// Extender filtros nas telas de Busca
document.querySelectorAll('.filter-title').forEach(item => {
    item.addEventListener('click', () => {
        const content = item.nextElementSibling;
        content.style.display = content.style.display === 'block' ? 'none' : 'block';
        item.classList.toggle('active');
    });
});

//Ver mais em detalhes do produto
document.getElementById("toggle-button").addEventListener("click", function () {
    var moreContent = document.getElementById("versaocompleta");
    var button = document.getElementById("toggle-button");

    if (moreContent.classList.contains("hidden")) {
        moreContent.classList.remove("hidden");
        button.textContent = "Mostrar menos";
    } else {
        moreContent.classList.add("hidden");
        button.textContent = "Ler descrição completa";
    }

});

// Quantidade de avaliações
const totalRatings = 10;

// Distribuição das avaliações
const ratingsData = {
    5: 8,
    4: 1,
    3: 0,
    2: 0,
    1: 0
};

// Atualiza a largura das barras de progresso
for (let rating in ratingsData) {
    const percentage = (ratingsData[rating] / totalRatings) * 100;
    document.getElementById(`bar-${rating}`).style.width = `${percentage}%`;
}

// Exibe o total de avaliações
document.getElementById('total-ratings').innerText = totalRatings;
