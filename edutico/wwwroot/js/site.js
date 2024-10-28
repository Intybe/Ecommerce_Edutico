document.addEventListener('DOMContentLoaded', function () {




    // Ver mais em detalhes do produto
    const toggleButton = document.getElementById("toggle-button");
    const moreContent = document.getElementById("versaocompleta");

    if (toggleButton && moreContent) {
        toggleButton.addEventListener("click", function () {
            if (moreContent.classList.contains("hidden")) {
                moreContent.classList.remove("hidden");
                toggleButton.textContent = "Mostrar menos";
            } else {
                moreContent.classList.add("hidden");
                toggleButton.textContent = "Ler descrição completa";
            }
        });
    } else {
        console.error("Elemento #toggle-button ou #versaocompleta não foi encontrado.");
    }



    // Extender filtros nas telas de Busca
    document.querySelectorAll('.filter-title').forEach(item => {
        item.addEventListener('click', () => {
            const content = item.nextElementSibling;
            content.style.display = content.style.display === 'block' ? 'none' : 'block';
            item.classList.toggle('active');
        });
    });

    // função para editar campos da tela Meu Perfil
    function edicao(botao, ids) {
        //loop para enqunto tiver campos no array
        ids.forEach(function (id) {
            var inputField = document.getElementById(id);

            if (inputField.hasAttribute('disabled')) {
                inputField.removeAttribute('disabled');
                inputField.focus();
                botao.textContent = 'Salvar';
            } else {
                inputField.setAttribute('disabled', true);
                botao.textContent = 'Modificar';
            }
        });
    }
    /*Trocar imagem na tela Detalhes produto*/
    // Seleciona todas as imagens pequenas
    const imgsPequenas = document.querySelectorAll('.imgpequena');
    // Seleciona a imagem principal
    const imgPrincipal = document.querySelector('.imgprincipal');

    // Adiciona um evento de clique a cada imagem pequena
    imgsPequenas.forEach(img => {
        img.addEventListener('click', function () {
            // Atualiza o src da imgprincipal para o src da imagem pequena clicada
            imgPrincipal.src = this.src;
        });
    });

    function selecionarHabilidade(chip) {
        // Adiciona ou remove a classe 'selected' ao chip
        chip.classList.toggle('selected');

        // Obtém todas as habilidades selecionadas
        const habilidadesSelecionadas = document.getElementById('habilidadesSelecionadas');
        const selectedChips = document.querySelectorAll('.chip.selected');

        // Cria um array com as habilidades selecionadas
        const habilidades = Array.from(selectedChips).map(chip => chip.textContent);

        // Atualiza o campo escondido com as habilidades selecionadas
        habilidadesSelecionadas.value = habilidades.join(', ');
    }




});
