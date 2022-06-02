; compiled by godbolt.org, edited by SirBrahms

test:
        push %ebp
        mov %esp,%ebp
        mov $99,%eax
        jmp .L1
.L1:
        leave
        ret
main:
        push %ebp
        mov %esp,%ebp
        sub $12,%esp
        mov $55,-4(%ebp)
        mov $12,-8(%ebp)
        mov -8(%ebp),%edx
        add %edx,-4(%ebp)
        mov -8(%ebp),%edx
        cmp -4(%ebp),%edx
        jne .L3
        call test
        mov %eax,-12(%ebp)
.L3:
        xor %eax,%eax
        jmp .L2
.L2:
        leave
        ret